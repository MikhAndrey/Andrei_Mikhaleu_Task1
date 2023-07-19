import {Component, Input, OnInit} from '@angular/core';
import {MapDirectionsService} from '@angular/google-maps';
import {map, Observable} from 'rxjs';

@Component({
  selector: 'app-readonly-route-map',
  templateUrl: './readonly-route-map.component.html',
})
export class ReadonlyRouteMapComponent implements OnInit {
  center: google.maps.LatLngLiteral = {
    lat: 51.5805,
    lng: 0
  };
  zoom: number = 5;

  directionsResults: Observable <google.maps.DirectionsResult | undefined> | undefined;

  @Input() markerPositions: google.maps.LatLngLiteral[] = [];

  protected mapDirectionsService: MapDirectionsService;
  constructor(mapDirectionsService: MapDirectionsService) {
    this.mapDirectionsService = mapDirectionsService;
  }

  ngOnInit() {
    if (this.markerPositions) {
      this.buildRoute();
    }
  }

  protected buildRoute(){
    const request: google.maps.DirectionsRequest = {
      destination: this.markerPositions.at(-1)!,
      origin: this.markerPositions[0],
      waypoints: this.markerPositions.slice(1, -1).map(el => {
       return {
         location: el,
         stopover: true
       }
      }),
      travelMode: google.maps.TravelMode.DRIVING
    };
    this.retrieveDirectionsResultsFromRequest(request);
  }

  protected retrieveDirectionsResultsFromRequest(request: google.maps.DirectionsRequest): void {
    this.directionsResults = this.mapDirectionsService.route(request).pipe(
      map(response => {
        if (response.status !== "OK") {
          alert("Imposssible to build such root!");
        }
        return response.result
      })
    );
  }
}
