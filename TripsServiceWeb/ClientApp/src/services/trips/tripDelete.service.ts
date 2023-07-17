import {Injectable} from "@angular/core";
import {Subject} from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class TripDeleteService {
  public tripIdToDelete$: Subject<number> = new Subject<number>();

  public setTripIdToDelete(tripIdToDelete: number) {
    this.tripIdToDelete$.next(tripIdToDelete);
  }
}
