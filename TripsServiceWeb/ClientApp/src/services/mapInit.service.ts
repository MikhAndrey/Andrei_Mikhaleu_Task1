import {Injectable} from "@angular/core";
import {Subject} from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class MapInitService {
  public markerPositions$: Subject<google.maps.LatLngLiteral[]> = new Subject<google.maps.LatLngLiteral[]>;

  public setMarkerPositions(markerPositions: google.maps.LatLngLiteral[]) {
    this.markerPositions$.next(markerPositions);
  }
}
