import {Component, OnDestroy, OnInit} from '@angular/core';
import {TripsService} from "../../services/trips/trips.service";
import {TripReadDTO} from "../../models/trips";
import {TripIdService} from "../../services/trips/tripId.service";
import {Observable, Subscription} from "rxjs";
import {TripDeleteService} from "../../services/trips/tripDelete.service";
import {TripFeedbackAddService} from "../../services/trips/tripFeedbackAdd.service";

@Component({
  selector: 'app-trips-list-base',
  templateUrl: './trips-list-base.component.html'
})
export class TripsListBaseComponent implements OnInit, OnDestroy{
  trips: TripReadDTO[] = [];
  protected tripIdToDeleteSubscription: Subscription;
  protected tripIdToAddFeedbackSubscription: Subscription;
  protected dataSourceObservable: Observable<TripReadDTO[]>;

  protected pageHeader: string;

  constructor(
    protected tripService: TripsService,
    protected tripIdService: TripIdService,
    protected tripDeleteService: TripDeleteService,
    protected tripFeedbackAddService: TripFeedbackAddService) {
  }

  ngOnInit(): void {
    this.dataSourceObservable.subscribe({
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

  handleRatingChange( ratingOptions: {tripId: number, rating: number }): void {
    const tripWithRequiredId: TripReadDTO | undefined = this.trips.find(trip => trip.id === ratingOptions.tripId);
    if (tripWithRequiredId) {
      tripWithRequiredId.rating = ratingOptions.rating;
      tripWithRequiredId.isNeedToBeRated = false;
    }
  }

}
