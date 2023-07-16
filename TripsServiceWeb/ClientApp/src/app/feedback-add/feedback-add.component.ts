import {Component, ElementRef, OnDestroy, OnInit, QueryList, ViewChild, ViewChildren} from '@angular/core';
import {FeedbackCreateDTO} from "../../models/feedbacks";
import {FeedbacksService} from "../../services/feedback.service";
import {TripIdService} from "../../services/tripId.service";
import {Subscription} from "rxjs";
import {maxRating} from "../appConstants";

@Component({
  selector: 'app-feedback-add-modal',
  templateUrl: './feedback-add.component.html'
})
export class FeedbackAddComponent implements OnInit, OnDestroy {
  feedback: FeedbackCreateDTO = new FeedbackCreateDTO();

  tripId: number;
  private tripIdSubscription: Subscription;

  starsCount: any[] = new Array(maxRating);

  @ViewChildren('stars') stars: QueryList<ElementRef>;
  @ViewChildren('filledStars') filledStars: QueryList<ElementRef>;
  @ViewChild('closeModalButton') closeModalButton: ElementRef;

  private starsNeedReset: boolean = true;

  constructor(private feedbackService: FeedbacksService, private tripIdService: TripIdService) {
  }

  ngOnInit() {
    this.tripIdSubscription = this.tripIdService.tripId$.subscribe((tripId) => this.tripId = tripId);
  }
  ngOnDestroy(): void {
    this.tripIdSubscription.unsubscribe();
  }

  addFeedback() {
    if (this.feedback.Rating != 0 || confirm("You are going to rate this trip as 0 stars. Continue?")) {
      this.feedback.TripId = this.tripId;
      this.feedbackService.add(this.feedback).subscribe({
        next: () => {
          this.closeModalButton.nativeElement.click();
          //Insert new feedback data to the parent page
        },
        error: (error) => {
          alert(error);
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
