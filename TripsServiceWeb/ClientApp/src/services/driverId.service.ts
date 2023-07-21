import {Injectable} from "@angular/core";
import {Subject} from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class DriverIdService {
  public driverId$: Subject<number | undefined> = new Subject<number | undefined>();

  public setDriverId(driverId: number | undefined) {
    this.driverId$.next(driverId);
  }
}
