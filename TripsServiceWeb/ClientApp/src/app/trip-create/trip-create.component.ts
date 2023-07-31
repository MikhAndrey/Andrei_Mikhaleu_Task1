import { Component, OnInit } from '@angular/core';
import {TripCreateDTO, TripCreateValidationErrors} from "../../models/trips";
import {TripsService} from "../../services/trips/trips.service";
import {RedirectService} from "../../services/redirect.service";
import {DriverInfoDTO} from "../../models/drivers";
import {ImageFile} from "../../models/images";
import {Observable} from "rxjs";

@Component({
  selector: 'app-trip-create',
  templateUrl: './trip-create.component.html',
  styleUrls: ['./trip-create.component.css']
})
export class TripCreateComponent implements OnInit {
  trip: TripCreateDTO = new TripCreateDTO();
  durationText?: string;
  driverName?: string;
  validationErrors: TripCreateValidationErrors = {};
  localEndTimeWithoutSeconds: string;
  protected tripSubmitMethod: Observable<TripCreateDTO>;

  constructor(protected tripService: TripsService, protected redirectService: RedirectService) { }

  ngOnInit(): void {
  }

  protected setTripSubmitMethod(form: HTMLFormElement){
    const formData: FormData = new FormData(form);
    this.tripSubmitMethod = this.tripService.add(this.trip, formData);
  }

  submit(form: HTMLFormElement): void {
    this.setTripSubmitMethod(form);
    this.tripSubmitMethod.subscribe({
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

  handleSelectedDriverChanged(driver: DriverInfoDTO): void {
    this.trip.driverId = driver.id;
    this.driverName = driver.name;
  }
}
