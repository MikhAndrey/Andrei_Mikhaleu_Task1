import {Injectable} from "@angular/core";
import {RoutePointDTO} from "../models/routePoints";

@Injectable({ providedIn: 'root' })
export class RoutesService {
  mapRoutePointsToCoordinates(routePoints: RoutePointDTO[]): google.maps.LatLngLiteral[]{
    return routePoints.sort((a, b) => a.ordinal - b.ordinal).map(rp => {
      return {
        lat: rp.latitude,
        lng: rp.longitude
      }
    });
  }
}
