import { Component, OnInit } from '@angular/core';
import {
  PastTripEditDTO,
  PastTripEditValidationErrors
} from "../../models/trips";
import {TripsService} from "../../services/trips/trips.service";
import {RedirectService} from "../../services/redirect.service";
import {ActivatedRoute} from "@angular/router";
import {ImagesService} from "../../services/images.service";

@Component({
  selector: 'app-trip-edit-past',
  templateUrl: './trip-edit-past.component.html',
})
export class TripEditPastComponent implements OnInit {
  trip: PastTripEditDTO = new PastTripEditDTO();
  validationErrors: PastTripEditValidationErrors = {};

  constructor(
    private tripService: TripsService,
    private redirectService: RedirectService,
    private route: ActivatedRoute,
    private imagesService: ImagesService) { }

  ngOnInit(): void {
    const id: number = parseInt(this.route.snapshot.paramMap.get('id')!);
    this.tripService.getTripForPastEditing(id).subscribe({
      next: (response) => {
        this.trip = response;
      },
      error: (error) => alert(error.error)
    });
  }

  submit(form: HTMLFormElement): void {
    const formData: FormData = new FormData(form);
    this.tripService.editPast(this.trip, formData).subscribe({
      next: () => {
        this.redirectService.redirectToAddress("trips/");
      },
      error: (error) => {
        this.validationErrors = error.error.errors || error.error;
      }
    });
  }

  handleFilesChanged(files: { file: File, url: string }[]): void {
    this.trip.imagesAsFiles = files.map(fileInfo => fileInfo.file);
  }

  deleteTripImage(id: number): void {
    this.imagesService.delete(id, this.trip.id).subscribe({
      next: () => this.trip.images = this.trip.images.filter(image => image.id !== id),
      error: (error) => alert(error.error)
    })
  }
}
