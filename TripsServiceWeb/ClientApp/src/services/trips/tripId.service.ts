import {Injectable} from "@angular/core";
import {Subject} from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class TripIdService {
  public tripId$: Subject<number> = new Subject<number>();

  public setTripId(tripId: number) {
    this.tripId$.next(tripId);
  }
}
