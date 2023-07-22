import {Injectable} from "@angular/core";
import {Subject} from "rxjs";
import {FeedbackReadDTO} from "../../models/feedbacks";

@Injectable({
  providedIn: 'root',
})
export class TripFeedbackAddService {
  public tripIdToAddFeedback$: Subject<number> = new Subject<number>();
  public addedFeedback$: Subject<FeedbackReadDTO> = new Subject<FeedbackReadDTO>();

  public setTripIdToAddFeedback(tripIdToAddFeedback: number): void {
    this.tripIdToAddFeedback$.next(tripIdToAddFeedback);
  }

  public setAddedFeedback(feedback: FeedbackReadDTO): void {
    this.addedFeedback$.next(feedback);
  }
}
