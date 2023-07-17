import {Component, OnDestroy, OnInit} from '@angular/core';
import {TripsService} from "../../services/trips/trips.service";
import {TripReadDTO} from "../../models/trips";
import {TripIdService} from "../../services/trips/tripId.service";
import {Subscription} from "rxjs";
import {TripDeleteService} from "../../services/trips/tripDelete.service";
import {TripFeedbackAddService} from "../../services/trips/tripFeedbackAdd.service";

@Component({
  selector: 'app-trips-list',
  templateUrl: './trips-list.component.html'
})
export class TripsListComponent implements OnInit, OnDestroy{
  trips: TripReadDTO[] = [];
  private tripIdToDeleteSubscription: Subscription;
  private tripIdToAddFeedbackSubscription: Subscription;

  constructor(
    private tripService: TripsService,
    private tripIdService: TripIdService,
    private tripDeleteService: TripDeleteService,
    private tripFeedbackAddService: TripFeedbackAddService) {
  }

  ngOnInit(): void {
    this.tripService.getAll().subscribe({
      next: (response) => {
        this.trips = response;
      },
      error: (error) => {
        alert(error.error);
      }
    });
    this.tripIdToDeleteSubscription = this.tripDeleteService.tripIdToDelete$.subscribe(
      (tripIdToDelete) => this.trips = this.trips.filter(trip => trip.id !== tripIdToDelete));
    this.tripIdToAddFeedbackSubscription = this.tripFeedbackAddService.tripIdToAddFeedback$.subscribe(
      (tripIdToAddFeedback): void => {
        const trip: TripReadDTO | undefined = this.trips.find(trip => trip.id === tripIdToAddFeedback);
        if (trip)
          trip.isNeedToBeRated = false;
      });
  }

  ngOnDestroy(): void {
    this.tripIdToDeleteSubscription.unsubscribe();
    this.tripIdToAddFeedbackSubscription.unsubscribe();
  }

  setTripId(tripId: number): void {
    this.tripIdService.setTripId(tripId);
  }

}
