import {
  Component,
  ElementRef,
  EventEmitter,
  OnDestroy,
  OnInit,
  Output,
  QueryList,
  ViewChild,
  ViewChildren
} from '@angular/core';
import {FeedbackCreateDTO} from "../../models/feedbacks";
import {FeedbacksService} from "../../services/feedback.service";
import {TripIdService} from "../../services/trips/tripId.service";
import {Subscription} from "rxjs";
import {maxRating} from "../appConstants";
import {TripFeedbackAddService} from "../../services/trips/tripFeedbackAdd.service";

@Component({
  selector: 'app-feedback-add-modal',
  templateUrl: './feedback-add.component.html'
})
export class FeedbackAddComponent implements OnInit, OnDestroy {
  feedback: FeedbackCreateDTO = new FeedbackCreateDTO();

  private tripIdSubscription: Subscription;

  starsCount: any[] = new Array(maxRating);

  @ViewChildren('stars') stars: QueryList<ElementRef>;
  @ViewChildren('filledStars') filledStars: QueryList<ElementRef>;
  @ViewChild('modalCloseButton') modalCloseButton: ElementRef;

  @Output() ratingChanged: EventEmitter<{ tripId: number, rating: number }> = new EventEmitter();

  private starsNeedReset: boolean = true;

  constructor(
    private feedbackService: FeedbacksService,
    private tripIdService: TripIdService,
    private tripFeedbackAddService: TripFeedbackAddService) {
  }

  ngOnInit() {
    this.tripIdSubscription = this.tripIdService.tripId$.subscribe((tripId) => this.feedback.TripId = tripId);
  }
  ngOnDestroy(): void {
    this.tripIdSubscription.unsubscribe();
  }

  addFeedback() {
    if (this.feedback.Rating != 0 || confirm("You are going to rate this trip as 0 stars. Continue?")) {
      this.feedbackService.add(this.feedback).subscribe({
        next: (response) => {
          this.tripFeedbackAddService.setTripIdToAddFeedback(this.feedback.TripId);
          this.tripFeedbackAddService.setAddedFeedback(response);
          this.modalCloseButton.nativeElement.click();
          this.ratingChanged.emit({tripId: this.feedback.TripId, rating: this.feedback.Rating});
        },
        error: (error) => {
          alert(error.error);
        }
      });
    }
  }

  starMouseEnter(index: number){
    for (let i = 0; i <= index; i++){
      this.stars.get(i)!.nativeElement.classList.add('star-hovered');
      this.filledStars.get(i)!.nativeElement.style.width = '100%';
    }
    for (let i = index + 1; i < this.filledStars.length; i++){
      this.stars.get(i)!.nativeElement.classList.remove('star-hovered');
      this.filledStars.get(i)!.nativeElement.style.width = '0%';
    }
    this.starsNeedReset = true;
  }

  starMouseLeave(index: number): void {
    if (this.starsNeedReset) {
      for (let i = 0; i <= index; i++) {
        this.stars.get(i)!.nativeElement.classList.remove('star-hovered');
        this.filledStars.get(i)!.nativeElement.style.width = '0%';
      }
    }
  }

  starMouseClick(index: number): void {
    this.feedback.Rating = index + 1;
    this.starsNeedReset = false;
  }
}
