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
  validationErrors: TripCreateValidationErrors = {};

  constructor(private tripService: TripsService, private redirectService: RedirectService) { }

  ngOnInit(): void {
  }

  handleFilesChanged(files: { file: File, url: string }[]) {
    this.trip.ImagesAsFiles = files;
  }

  submit(): void {
    this.tripService.add(this.trip).subscribe({
      next: () => {
        this.redirectService.redirectToAddress("trips/");
      },
      error: (error) => {
        this.validationErrors = error.error.errors || error.error;
      }
    });
  }
}
