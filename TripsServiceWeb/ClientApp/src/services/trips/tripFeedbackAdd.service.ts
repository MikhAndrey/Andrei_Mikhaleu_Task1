import {Injectable} from "@angular/core";
import {Subject} from "rxjs";
import {FeedbackCreateDTO} from "../../models/feedbacks";

@Injectable({
  providedIn: 'root',
})
export class TripFeedbackAddService {
  public tripIdToAddFeedback$: Subject<number> = new Subject<number>();
  public addedFeedback$: Subject<FeedbackCreateDTO> = new Subject<FeedbackCreateDTO>();

  public setTripIdToAddFeedback(tripIdToAddFeedback: number): void {
    this.tripIdToAddFeedback$.next(tripIdToAddFeedback);
  }

  public setAddedFeedback(feedback: FeedbackCreateDTO): void {
    this.addedFeedback$.next(feedback);
  }
}
