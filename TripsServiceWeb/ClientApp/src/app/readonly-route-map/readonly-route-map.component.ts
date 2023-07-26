import {Component, OnDestroy} from '@angular/core';
import {MapDirectionsService} from '@angular/google-maps';
import {map, Observable, Subscription} from 'rxjs';
import {MapInitService} from "../../services/mapInit.service";
import {mapCenter, mapZoom} from "../appConstants";

@Component({
  selector: 'app-readonly-route-map',
  templateUrl: './readonly-route-map.component.html',
})
export class ReadonlyRouteMapComponent implements OnDestroy {
  center: google.maps.LatLngLiteral = mapCenter;
  zoom: number = mapZoom;

  directionsResults: Observable<google.maps.DirectionsResult | undefined> | undefined;

  markerPositions: google.maps.LatLngLiteral[] = [];
  protected markerPositionsSubscription: Subscription;

  protected mapDirectionsService: MapDirectionsService;

  constructor(mapDirectionsService: MapDirectionsService, protected mapInitService: MapInitService) {
    this.mapDirectionsService = mapDirectionsService;
    this.markerPositionsSubscription = this.mapInitService.markerPositions$.subscribe((markerPositions) => this.initializeMapWithMarkerPositions(markerPositions));
  }

  private initializeMapWithMarkerPositions(markerPositions: google.maps.LatLngLiteral[]) {
    this.markerPositions = markerPositions;
    this.buildRoute();
  }

  protected buildRoute() {
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

  ngOnDestroy() {
    this.markerPositionsSubscription.unsubscribe();
  }
}
