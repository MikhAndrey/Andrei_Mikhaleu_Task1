import { Component, OnInit } from '@angular/core';
import {MapDirectionsService, MapMarker} from '@angular/google-maps';
import { map, Observable } from 'rxjs';

@Component({
  selector: 'app-route-map',
  templateUrl: './route-map.component.html',
})
export class RouteMapComponent implements OnInit {
  ngOnInit(): void {}

  center: google.maps.LatLngLiteral = {
    lat: 51.5805,
    lng: 0
  };

  zoom = 8;
  directionsResults: Observable <google.maps.DirectionsResult | undefined> | undefined;

  markerOptions: google.maps.MarkerOptions = {
    draggable: true,
    clickable: true
  };

  origin: google.maps.LatLngLiteral = {lat: 0, lng: 0};
  destination: google.maps.LatLngLiteral = {lat: 0, lng: 0};
  waypoints: google.maps.DirectionsWaypoint[] = [];

  markerPositions: google.maps.LatLngLiteral[] = [];

  mapDirectionsService: MapDirectionsService;
  constructor(mapDirectionsService: MapDirectionsService) {
    this.mapDirectionsService = mapDirectionsService;
  }

  buildRoute(){
    const request: google.maps.DirectionsRequest = {
      destination: this.destination,
      origin: this.origin,
      waypoints: this.waypoints,
      travelMode: google.maps.TravelMode.DRIVING
    };
    this.directionsResults = this.mapDirectionsService.route(request).pipe(map(response => response.result));
  }

  addMarker(event: google.maps.MapMouseEvent){
    if (event.latLng != null) {
      this.markerPositions.push(event.latLng.toJSON());
      this.waypoints.push({
        location: event.latLng,
        stopover: true
      });
      this.destination = this.markerPositions[this.markerPositions.length - 1];
      this.origin = this.markerPositions[0];
      this.buildRoute();
    }
  }

  onMarkerClick(index: number){
    this.markerPositions.splice(index, 1);
    this.waypoints.splice(index, 1);
    this.destination = this.markerPositions[this.markerPositions.length - 1];
    this.origin = this.markerPositions[0];
    this.buildRoute();
  }

  onMarkerDragEnd(event: DragEvent, index: number) {
    const googleEvent = event as unknown as google.maps.MapMouseEvent;
    if (googleEvent.latLng != null) {
      const newLatLng = googleEvent.latLng.toJSON();

      // Обновить позицию маркера в массиве
      this.markerPositions[index] = newLatLng;

      // Построить маршрут
      this.buildRoute();

      // Вывести индекс и новые координаты маркера
      console.log('Индекс маркера:', index);
      console.log('Новые координаты маркера:', newLatLng);
    }
  }
}
