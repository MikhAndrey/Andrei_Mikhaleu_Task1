import { Component, OnInit } from '@angular/core';
import {TripCreateDTO, TripCreateValidationErrors} from "../../models/trips";

@Component({
  selector: 'app-trip-create',
  templateUrl: './trip-create.component.html',
  styleUrls: ['./trip-create.component.css']
})
export class TripCreateComponent implements OnInit {
  trip: TripCreateDTO = new TripCreateDTO();
  validationErrors: TripCreateValidationErrors = {};

  constructor() { }

  ngOnInit(): void {
  }

  handleFilesChanged(files: { file: File, url: string }[]) {
    this.trip.ImagesAsFiles = files;
  }
}
