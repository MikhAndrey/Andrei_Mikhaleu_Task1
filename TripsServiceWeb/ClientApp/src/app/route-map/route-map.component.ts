import {Component} from '@angular/core';
import {MapDirectionsService} from '@angular/google-maps';
import {map, Observable} from 'rxjs';

@Component({
  selector: 'app-route-map',
  templateUrl: './route-map.component.html',
})
export class RouteMapComponent {
  center: google.maps.LatLngLiteral = {
    lat: 51.5805,
    lng: 0
  };
  zoom: number = 8;

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
  markersCount: number = 0;

  mapDirectionsService: MapDirectionsService;
  constructor(mapDirectionsService: MapDirectionsService) {
    this.mapDirectionsService = mapDirectionsService;
  }

  buildRoute(){
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
        return response.result
      })
    );
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
}
