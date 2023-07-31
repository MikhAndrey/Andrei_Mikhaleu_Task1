import {Component, OnInit} from '@angular/core';
import {TripEditDTO} from "../../models/trips";
import {TripsService} from "../../services/trips/trips.service";
import {RedirectService} from "../../services/redirect.service";
import {RoutesService} from "../../services/routes.service";
import {ActivatedRoute} from "@angular/router";
import {ImagesService} from "../../services/images.service";
import {MapInitService} from "../../services/mapInit.service";
import {DriverIdService} from "../../services/driverId.service";
import {TripCreateComponent} from "../trip-create/trip-create.component";
import {Observable} from "rxjs";

@Component({
  selector: 'app-trip-edit',
  templateUrl: './trip-edit.component.html',
})
export class TripEditComponent extends TripCreateComponent implements OnInit {
  trip: TripEditDTO = new TripEditDTO();
  protected tripInitMethod: Observable<TripEditDTO>;
  declare tripSubmitMethod: Observable<TripEditDTO>;

  constructor(
    protected tripService: TripsService,
    protected redirectService: RedirectService,
    protected routesService: RoutesService,
    route: ActivatedRoute,
    protected imagesService: ImagesService,
    protected mapInitService: MapInitService,
    protected driverIdService: DriverIdService) {
    super(tripService, redirectService);
    const id: number = parseInt(route.snapshot.paramMap.get('id')!)
    this.tripInitMethod = this.tripService.getTripForCurrentEditing(id);
  }

  override ngOnInit(): void {
    this.tripInitMethod.subscribe({
      next: (response) => {
        this.parseIncomingTripData(response);
      },
      error: (error) => alert(error.error)
    });
  }

  override setTripSubmitMethod(form: HTMLFormElement) {
    const formData: FormData = new FormData(form);
    this.tripSubmitMethod = this.tripService.editCurrent(this.trip, formData);
  }

  protected parseIncomingTripData(trip: TripEditDTO) {
    this.trip = trip;
    this.driverIdService.setDriverId(trip.driverId);
    this.trip.endTime = new Date(trip.endTime!);
    const markerPositions: google.maps.LatLngLiteral[] = this.routesService.mapRoutePointsToCoordinates(trip.routePoints);
    this.mapInitService.setMarkerPositions(markerPositions);
  }

  deleteTripImage(id: number): void {
    this.imagesService.delete(id, this.trip.id).subscribe({
      next: () => this.trip.images = this.trip.images.filter(image => image.id !== id),
      error: (error) => alert(error.error)
    })
  }
}
