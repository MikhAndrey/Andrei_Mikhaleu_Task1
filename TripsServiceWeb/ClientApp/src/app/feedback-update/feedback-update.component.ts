import {
  AfterViewInit,
  Component,
  ElementRef, EventEmitter,
  Input,
  Output,
  QueryList,
  ViewChild,
  ViewChildren
} from '@angular/core';
import {FeedbackReadDTO} from "../../models/feedbacks";
import {FeedbacksService} from "../../services/feedback.service";
import {maxRating} from "../appConstants";

@Component({
  selector: 'app-feedback-update-modal',
  templateUrl: './feedback-update.component.html'
})
export class FeedbackUpdateComponent implements AfterViewInit {

  @Input() feedback: FeedbackReadDTO = new FeedbackReadDTO();
  @Output() feedbackChanged: EventEmitter<FeedbackReadDTO> = new EventEmitter();

  starsCount: any[] = new Array(maxRating);

  @ViewChildren('stars') stars: QueryList<ElementRef>;
  @ViewChildren('filledStars') filledStars: QueryList<ElementRef>;
  @ViewChild('modalCloseButton') modalCloseButton: ElementRef;

  private starsNeedReset: boolean = false;

  constructor(
    private feedbackService: FeedbacksService) {
  }

  ngAfterViewInit() {
    this.starMouseEnter(this.feedback.rating - 1);
  }

  ngOnDestroy(): void {

  }

  updateFeedback() {
    if (this.feedback.rating != 0 || confirm("You are going to rate this trip as 0 stars. Continue?")) {
      this.feedbackService.update(this.feedback).subscribe({
        next: () => {
          this.feedbackChanged.emit(this.feedback);
          this.modalCloseButton.nativeElement.click();
        },
        error: (error) => {
          alert(error.error);
        }
      });
    }
  }

  starMouseEnter(index: number) {
    for (let i = 0; i <= index; i++) {
      this.stars.get(i)!.nativeElement.classList.add('star-hovered');
      this.filledStars.get(i)!.nativeElement.style.width = '100%';
    }
    for (let i = index + 1; i < this.filledStars.length; i++) {
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
    this.feedback.rating = index + 1;
    this.starsNeedReset = false;
  }
}
