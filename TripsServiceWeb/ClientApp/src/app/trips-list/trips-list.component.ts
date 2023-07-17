import {Component, OnDestroy, OnInit} from '@angular/core';
import {TripsService} from "../../services/trips.service";
import {TripReadDTO} from "../../models/trips";
import {TripIdService} from "../../services/tripId.service";
import {Subscription} from "rxjs";
import {TripDeleteService} from "../../services/tripDelete.service";

@Component({
  selector: 'app-trips-list',
  templateUrl: './trips-list.component.html',
  styleUrls: ['./trips-list.component.css']
})
export class TripsListComponent implements OnInit, OnDestroy{
  trips: TripReadDTO[] = [];
  private tripIdToDeleteSubscription: Subscription;

  constructor(
    private tripService: TripsService,
    private tripIdService: TripIdService,
    private tripDeleteService: TripDeleteService) {
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
    this.tripIdToDeleteSubscription = this.tripDeleteService.tripIdToDelete$.subscribe(
      (tripIdToDelete) => this.trips = this.trips.filter(trip => trip.id !== tripIdToDelete));
  }

  ngOnDestroy(): void {
    this.tripIdToDeleteSubscription.unsubscribe();
  }

  setTripId(tripId: number): void {
    this.tripIdService.setTripId(tripId);
  }

}
