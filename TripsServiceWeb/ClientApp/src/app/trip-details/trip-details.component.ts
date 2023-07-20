import {Component, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {TripsService} from "../../services/trips/trips.service";
import {TripDetailsDTO} from "../../models/trips";
import {RoutesService} from "../../services/routes.service";
import {Subscription} from "rxjs";
import {TripCommentService} from "../../services/trips/tripComment.service";
import {TripIdService} from "../../services/trips/tripId.service";
import {TripFeedbackAddService} from "../../services/trips/tripFeedbackAdd.service";
import {CommentsService} from "../../services/comments.service";
import {FeedbacksService} from "../../services/feedback.service";
import {MapInitService} from "../../services/mapInit.service";

@Component({
  selector: 'app-trip-details',
  templateUrl: './trip-details.component.html',
  styleUrls: ['./trip-details.component.css', '../drivers-list/drivers-list-modal.component.css']
})
export class TripDetailsComponent implements OnInit, OnDestroy {

  trip: TripDetailsDTO = new TripDetailsDTO();

  private tripCommentAddSubscription: Subscription;
  private tripFeedbackAddSubscription: Subscription;

  constructor(
    private route: ActivatedRoute,
    private tripService: TripsService,
    private routesService: RoutesService,
    private tripCommentAddService: TripCommentService,
    private tripIdService: TripIdService,
    private tripFeedbackAddService: TripFeedbackAddService,
    private commentService: CommentsService,
    private feedbackService: FeedbacksService,
    private mapInitService: MapInitService
  ) {}

  ngOnInit(): void {
    const id = parseInt(this.route.snapshot.paramMap.get('id')!);
    this.tripService.getDetails(id).subscribe({
      next: (response) => {
        this.trip = response;
        const markerPositions: google.maps.LatLngLiteral[] = this.routesService.mapRoutePointsToCoordinates(this.trip.routePoints);
        this.mapInitService.setMarkerPositions(markerPositions);
      },
      error: (error) => {
        alert(error.error);
      }
    });
    this.tripCommentAddSubscription = this.tripCommentAddService.addedComment$.subscribe(
      (comment) => this.trip.comments.unshift(comment));
    this.tripFeedbackAddSubscription = this.tripFeedbackAddService.addedFeedback$.subscribe(
      (feedback): void => {
        this.trip.feedbackText = feedback.Text;
        this.trip.rating = feedback.Rating;
        this.trip.isNeedToBeRated = false;
      });
  }

  ngOnDestroy(): void {
    this.tripCommentAddSubscription.unsubscribe();
  }

  deleteComment(id: number): void {
    this.commentService.delete(id).subscribe({
      next: () => this.trip.comments = this.trip.comments.filter(comment => comment.id !== id),
      error: (error) => {
        alert(error.error);
      }
    })
  }

  deleteFeedback(id: number): void {
    this.feedbackService.delete(id).subscribe({
      next: () => this.trip.rating = undefined,
      error: (error) => {
        alert(error.error);
      }
    });
  }

  startTrip(tripId: number): void {
    this.tripService.startTrip(tripId).subscribe({
      next: (response) => {
        this.trip.isFuture = false;
        this.trip.isCurrent = true;
        this.tripService.updateTripDateInfo(this.trip, response);
      }
    });
  }

  finishTrip(tripId: number): void {
    this.tripService.finishTrip(tripId).subscribe({
      next: (response) => {
        this.trip.isCurrent = false;
        this.trip.isPast = true;
        this.tripService.updateTripDateInfo(this.trip, response);
      },
      error: (error) => {
        alert(error.error);
      }
    });
  }

  setTripIdForRating(): void {
    this.tripIdService.setTripId(this.trip.id);
  }
}
