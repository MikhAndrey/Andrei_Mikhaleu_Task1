import {Injectable} from "@angular/core";
import {Subject} from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class TripFeedbackAddService {
  public tripIdToAddFeedback$: Subject<number> = new Subject<number>();

  public setTripIdToAddFeedback(tripIdToAddFeedback: number) {
    this.tripIdToAddFeedback$.next(tripIdToAddFeedback);
  }
}
