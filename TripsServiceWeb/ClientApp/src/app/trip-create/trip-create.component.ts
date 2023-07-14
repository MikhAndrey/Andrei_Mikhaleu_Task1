import { Component, OnInit } from '@angular/core';
import {TripCreateDTO, TripCreateValidationErrors} from "../../models/trips";
import {TripsService} from "../../services/trips.service";
import {RedirectService} from "../../services/redirect.service";

@Component({
  selector: 'app-trip-create',
  templateUrl: './trip-create.component.html',
  styleUrls: ['./trip-create.component.css']
})
export class TripCreateComponent implements OnInit {
  trip: TripCreateDTO = new TripCreateDTO();
  durationText?: string;
  validationErrors: TripCreateValidationErrors = {};

  constructor(private tripService: TripsService, private redirectService: RedirectService) { }

  ngOnInit(): void {
  }

  submit(form: HTMLFormElement): void {
    const formData: FormData = new FormData(form);
    this.tripService.add(this.trip, formData).subscribe({
      next: () => {
        this.redirectService.redirectToAddress("trips/");
      },
      error: (error) => {
        this.validationErrors = error.error.errors || error.error;
      }
    });
  }

  handleFilesChanged(files: { file: File, url: string }[]): void {
    this.trip.ImagesAsFiles = files.map(fileInfo => fileInfo.file);
  }
  handleDurationTextChanged(durationText?: string): void {
    this.durationText = durationText;
  }
  handleStartTimeZoneChanged(startTimeZoneOffset: number): void {
    this.trip.StartTimeZoneOffset = startTimeZoneOffset;
  }
  handleFinishTimeZoneOffsetChanged(finishTimeZoneOffset: number): void {
    this.trip.FinishTimeZoneOffset = finishTimeZoneOffset;
  }
  handleFinishTimeChanged(finishTime: Date): void {
    this.trip.EndTime = finishTime;
  }
  handleDistanceChanged(distance: number): void {
    this.trip.Distance = distance;
  }
  handleRoutePointsChanged(routePoints: string): void {
    this.trip.RoutePointsAsString = routePoints;
  }
}
