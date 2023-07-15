import {Component, Input} from '@angular/core';
import {FeedbackCreateDTO} from "../../models/feedbacks";
import {FeedbacksService} from "../../services/feedback.service";

@Component({
  selector: 'app-feedback-add-modal',
  templateUrl: './feedback-add.component.html'
})
export class FeedbackAddComponent {
  feedback: FeedbackCreateDTO = new FeedbackCreateDTO();

  @Input() tripId: number = 0;

  private formStars: NodeListOf<HTMLElement> = document.querySelectorAll("form .star") as NodeListOf<HTMLElement>;
  private filledStars: NodeListOf<HTMLElement> = document.querySelectorAll(".star-filled") as NodeListOf<HTMLElement>;

  private starsNeedReset: boolean = true;

  constructor(private feedbackService: FeedbacksService) {
  }

  addFeedback() {
    if (this.feedback.Rating != 0 || confirm("You are going to rate this trip as 0 stars. Continue?")) {
      this.feedback.TripId = this.tripId;
      this.feedbackService.add(this.feedback).subscribe({
        next: () => {
          //Insert new feedback data to the parent page
        },
        error: (error) => {
          alert(error);
        }
      });
    }
  }

  starMouseEnter(starElement: HTMLElement, index: number){
    for (let i = 0; i <= index; i++){
      starElement.classList.add('star-hovered');
      this.filledStars[i].style.width = '100%';
    }
    for (let i = index + 1; i < this.filledStars.length; i++){
      this.formStars[i].classList.remove('star-hovered');
      this.filledStars[i].style.width = '0%';
    }
    this.starsNeedReset = true;
  }

  starMouseLeave(starElement: HTMLElement, index: number): void {
    if (this.starsNeedReset) {
      for (let i = 0; i <= index; i++) {
        this.formStars[i].classList.remove('star-hovered');
        this.filledStars[i].style.width = '0%';
      }
    }
  }

  starMouseClick(index: number): void {
    this.feedback.Rating = index + 1;
    this.starsNeedReset = false;
  }
}
