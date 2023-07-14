import {Component, EventEmitter, Input, Output} from '@angular/core';
import {MapDirectionsService} from '@angular/google-maps';
import {map, Observable} from 'rxjs';
import {formatDurationInSeconds} from "../../utils/formatDuration";
import {environment} from "../../environments/environment";

@Component({
  selector: 'app-route-map',
  templateUrl: './route-map.component.html',
})
export class RouteMapComponent {
  center: google.maps.LatLngLiteral = {
    lat: 51.5805,
    lng: 0
  };
  zoom: number = 5;

  directionsResults: Observable <google.maps.DirectionsResult | undefined> | undefined;

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

  markerPositions: google.maps.LatLngLiteral[] = [];
  private markersCount: number = 0;

  private mapDirectionsService: MapDirectionsService;
  constructor(mapDirectionsService: MapDirectionsService) {
    this.mapDirectionsService = mapDirectionsService;
  }

  @Output() durationTextChanged: EventEmitter<string> = new EventEmitter();
  private _durationText?: string;

  @Output() distanceChanged: EventEmitter<number> = new EventEmitter();
  private _distance: number = 0;

  @Output() startTimeZoneOffsetChanged: EventEmitter<number> = new EventEmitter();
  private _startTimeZoneOffset?: number;

  @Output() finishTimeZoneOffsetChanged: EventEmitter<number> = new EventEmitter();
  private _finishTimeZoneOffset?: number;

  @Output() finishTimeChanged: EventEmitter<Date> = new EventEmitter();
  @Input() private _finishTime?: Date;

  @Output() routePointsChanged: EventEmitter<string> = new EventEmitter();
  @Input() private _routePoints?: string;
  set durationText(value: string){
    this._durationText = value;
    this.durationTextChanged.emit(this._durationText);
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
    this._finishTime = value;
    this.finishTimeChanged.emit(this._finishTime);
  }

  set distance(value: number){
    this._distance = value;
    this.distanceChanged.emit(this._distance);
  }

  set routePoints(value: string){
    this._routePoints = value;
    this.routePointsChanged.emit(this._routePoints);
  }

  @Input() startTime?: Date;

  private buildRoute(){
    const request: google.maps.DirectionsRequest = {
      destination: this.markerPositions[this.markersCount - 1],
      origin: this.markerPositions[0],
      waypoints: this.markerPositions.slice(1, -1).map(el => {
       return {
         location: el,
         stopover: true
       }
      }),
      travelMode: google.maps.TravelMode.DRIVING
    };
    this.directionsResults = this.mapDirectionsService.route(request).pipe(
      map(response => {
        if (response.status !== "OK") {
          alert("Imposssible to build such root!");
          this.removeMarker(this.markersCount - 1);
        }
        this.getRouteData(response.result);
        return response.result
      })
    );
  }

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

    const finishTimeZoneInfo: {dstOffset: number, rawOffset: number} = await this.getTimeZoneInfo(this.markerPositions[this.markersCount - 1]);
    this.finishTimeZoneOffset = finishTimeZoneInfo.dstOffset + finishTimeZoneInfo.rawOffset;

    if (this.startTime) {
      const startTimeDate = new Date(this.startTime);
      this.finishTime = new Date(startTimeDate.getTime() + 1000 * (duration - this._startTimeZoneOffset! + this._finishTimeZoneOffset!));
    }

    this.routePoints = JSON.stringify(this.markerPositions.map(el => {
      return {
        latitude: el.lat,
        longitude: el.lng
      }
    }));
  }

  addMarker(event: google.maps.MapMouseEvent): void{
    if (event.latLng != null) {
      this.markerPositions.push(event.latLng.toJSON());
      this.markersCount ++;
      if (this.markersCount > 1)
        this.buildRoute();
    }
  }

  removeMarker(index: number): void{
    this.markerPositions.splice(index, 1);
    this.markersCount--;
    if (this.markersCount > 1)
      this.buildRoute();
    else
      this.directionsResults = undefined;
  }

  moveMarker(event: google.maps.MapMouseEvent, index: number) {
    if (event.latLng != null) {
      this.markerPositions[index] = event.latLng.toJSON();
      if (this.markersCount > 1)
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
