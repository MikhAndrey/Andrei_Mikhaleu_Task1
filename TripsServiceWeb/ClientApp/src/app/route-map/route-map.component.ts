import {Component, EventEmitter, Input, Output} from '@angular/core';
import {map} from 'rxjs';
import {formatDurationInSeconds} from "../../utils/formatDuration";
import {environment} from "../../environments/environment";
import {ReadonlyRouteMapComponent} from "../readonly-route-map/readonly-route-map.component";

@Component({
  selector: 'app-route-map',
  templateUrl: './route-map.component.html',
})
export class RouteMapComponent extends ReadonlyRouteMapComponent {

  markerOptions: google.maps.MarkerOptions = {
    draggable: true,
    clickable: true,
    icon: {
      path: google.maps.SymbolPath.CIRCLE,
      fillColor: "#ff0000",
      fillOpacity: 1,
      strokeColor: "#000000",
      strokeWeight: 1,
      scale: 7,
      labelOrigin: new google.maps.Point(0, 3),
    }
  };

  @Output() durationTextChanged: EventEmitter<string> = new EventEmitter();
  @Output() distanceChanged: EventEmitter<number> = new EventEmitter();
  @Output() startTimeZoneOffsetChanged: EventEmitter<number> = new EventEmitter();
  private _startTimeZoneOffset?: number;
  @Output() finishTimeZoneOffsetChanged: EventEmitter<number> = new EventEmitter();
  private _finishTimeZoneOffset?: number;
  @Output() finishTimeChanged: EventEmitter<Date> = new EventEmitter();
  @Output() routePointsChanged: EventEmitter<string> = new EventEmitter();

  set durationText(value: string){
    this.durationTextChanged.emit(value);
  }

  set startTimeZoneOffset(value: number){
    this._startTimeZoneOffset = value;
    this.startTimeZoneOffsetChanged.emit(this._startTimeZoneOffset);
  }

  set finishTimeZoneOffset(value: number){
    this._finishTimeZoneOffset = value;
    this.finishTimeZoneOffsetChanged.emit(this._finishTimeZoneOffset);
  }

  set finishTime(value: Date){
    this.finishTimeChanged.emit(value);
  }

  set distance(value: number){
    this.distanceChanged.emit(value);
  }

  set routePoints(value: string){
    this.routePointsChanged.emit(value);
  }

  @Input() startTime?: Date;

  private async getRouteData(routeData?: google.maps.DirectionsResult | undefined){
    const duration = routeData?.routes[0].legs.reduce((acc: number, el) => {
      acc += el.duration!.value;
      return acc;
    }, 0)!;
    this.durationText = formatDurationInSeconds(duration);

    this.distance = routeData?.routes[0].legs.reduce((acc: number, el) => {
      acc += el.distance!.value;
      return acc;
    }, 0)!/1000;

    const startTimeZoneInfo: {dstOffset: number, rawOffset: number} = await this.getTimeZoneInfo(this.markerPositions[0]);
    this.startTimeZoneOffset = startTimeZoneInfo.dstOffset + startTimeZoneInfo.rawOffset;

    const finishTimeZoneInfo: {dstOffset: number, rawOffset: number} = await this.getTimeZoneInfo(this.markerPositions.at(-1)!);
    this.finishTimeZoneOffset = finishTimeZoneInfo.dstOffset + finishTimeZoneInfo.rawOffset;

    if (this.startTime) {
      const startTimeDate = new Date(this.startTime);
      this.finishTime = new Date(startTimeDate.getTime() + 1000 * (duration - this._startTimeZoneOffset! + this._finishTimeZoneOffset!));
    }

    this.routePoints = JSON.stringify(this.markerPositions.map((el, index) => {
      return {
        Latitude: el.lat,
        Longitude: el.lng,
        Ordinal: index
      }
    }));
  }

  override retrieveDirectionsResultsFromRequest(request: google.maps.DirectionsRequest): void {
    this.directionsResults = this.mapDirectionsService.route(request).pipe(
      map(response => {
        if (response.status !== "OK") {
          alert("Imposssible to build such root!");
          this.removeMarker(-1);
        }
        this.getRouteData(response.result);
        return response.result
      })
    );
  }

  addMarkerAndBuildRoute(latLng: google.maps.LatLng | null): void{
    if (latLng != null) {
      this.markerPositions.push(latLng.toJSON());
      if (this.markerPositions[1])
        this.buildRoute();
    }
  }

  addMarker(latLng: google.maps.LatLng | null): void{
    if (latLng != null) {
      this.markerPositions.push(latLng.toJSON());
    }
  }

  removeMarker(index: number): void{
    this.markerPositions.splice(index, 1);
    if (this.markerPositions[1])
      this.buildRoute();
    else
      this.directionsResults = undefined;
  }

  moveMarker(latLng: google.maps.LatLng | null, index: number) {
    if (latLng != null) {
      this.markerPositions[index] = latLng.toJSON();
      if (this.markerPositions[1])
        this.buildRoute();
    }
  }

  private async getTimeZoneInfo(coordinates: google.maps.LatLngLiteral): Promise<any> {
    const apiKey: string = environment.GOOGLE_MAPS_API_KEY;
    const timestamp = Math.floor(new Date().getTime() / 1000);

    let requestUrl = `https://maps.googleapis.com/maps/api/timezone/json?location=${coordinates.lat},${coordinates.lng}&timestamp=${timestamp}&key=${apiKey}`;

    try {
      const response = await fetch(requestUrl);
      return response.json();
    } catch (error) {
      alert("Failed to retrieve timezone information: " + error);
    }
  }
}
