import { Component, OnInit } from '@angular/core';
import {TripCreateValidationErrors, TripEditDTO} from "../../models/trips";
import {TripsService} from "../../services/trips/trips.service";
import {RedirectService} from "../../services/redirect.service";
import {RoutesService} from "../../services/routes.service";
import {ActivatedRoute} from "@angular/router";
import {ImagesService} from "../../services/images.service";
import {MapInitService} from "../../services/mapInit.service";
import {DriverInfoDTO} from "../../models/drivers";
import {DriverIdService} from "../../services/driverId.service";
import {ImageFile} from "../../models/images";

@Component({
  selector: 'app-trip-edit',
  templateUrl: './trip-edit.component.html',
})
export class TripEditComponent implements OnInit {
  trip: TripEditDTO = new TripEditDTO();
  durationText?: string;
  driverName?: string;
  validationErrors: TripCreateValidationErrors = {};
  localEndTimeWithoutSeconds: string;

  constructor(
    private tripService: TripsService,
    private redirectService: RedirectService,
    private routesService: RoutesService,
    private route: ActivatedRoute,
    private imagesService: ImagesService,
    private mapInitService: MapInitService,
    private driverIdService: DriverIdService) { }

  ngOnInit(): void {
    const id: number = parseInt(this.route.snapshot.paramMap.get('id')!);
    this.tripService.getTripForCurrentEditing(id).subscribe({
      next: (response) => {
        this.trip = response;
        this.driverIdService.setDriverId(this.trip.driverId);
        this.trip.endTime = new Date(this.trip.endTime!);
        const markerPositions: google.maps.LatLngLiteral[] = this.routesService.mapRoutePointsToCoordinates(this.trip.routePoints);
        this.mapInitService.setMarkerPositions(markerPositions);
      },
      error: (error) => alert(error.error)
    });
  }

  submit(form: HTMLFormElement): void {
    const formData: FormData = new FormData(form);
    this.tripService.editCurrent(this.trip, formData).subscribe({
      next: () => {
        this.redirectService.redirectToAddress("trips/");
      },
      error: (error) => {
        this.validationErrors = error.error.errors || error.error;
      }
    });
  }

  handleFilesChanged(files: ImageFile[]): void {
    this.trip.imagesAsFiles = files.map(fileInfo => fileInfo.file);
  }
  handleDurationTextChanged(durationText?: string): void {
    this.durationText = durationText;
  }
  handleStartTimeZoneChanged(startTimeZoneOffset: number): void {
    this.trip.startTimeZoneOffset = startTimeZoneOffset;
  }
  handleFinishTimeZoneOffsetChanged(finishTimeZoneOffset: number): void {
    this.trip.finishTimeZoneOffset = finishTimeZoneOffset;
  }
  handleFinishTimeChanged(finishTime: Date): void {
    this.trip.endTime = finishTime;
    const options: Intl.DateTimeFormatOptions = { hour: 'numeric', minute: 'numeric' };
    this.localEndTimeWithoutSeconds = `${this.trip.endTime.toLocaleDateString()} ${this.trip.endTime.toLocaleTimeString([], options)}`;
  }
  handleDistanceChanged(distance: number): void {
    this.trip.distance = distance;
  }
  handleRoutePointsChanged(routePoints: string): void {
    this.trip.routePointsAsString = routePoints;
  }

  deleteTripImage(id: number): void {
    this.imagesService.delete(id, this.trip.id).subscribe({
      next: () => this.trip.images = this.trip.images.filter(image => image.id !== id),
      error: (error) => alert(error.error)
    })
  }

  handleSelectedDriverChanged(driver: DriverInfoDTO): void {
    this.trip.driverId = driver.id;
    this.driverName = driver.name;
  }
}
