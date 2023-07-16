import {Component, OnInit} from '@angular/core';
import {TripsService} from "../../services/trips.service";
import {TripReadDTO} from "../../models/trips";
import {TripIdService} from "../../services/tripId.service";

@Component({
  selector: 'app-trips-list',
  templateUrl: './trips-list.component.html',
  styleUrls: ['./trips-list.component.css']
})
export class TripsListComponent implements OnInit{
  trips: TripReadDTO[] = [];

  constructor(private tripService: TripsService, private tripIdService: TripIdService) {
  }

  ngOnInit(): void {
    this.tripService.getAll().subscribe({
      next: (response) => {
        this.trips = response;
      },
      error: (error) => {
        alert(error);
      }
    });
  }

  setTripId(tripId: number): void {
    this.tripIdService.setTripId(tripId);
  }

}
