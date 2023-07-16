import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {TripCreateDTO, TripReadDTO} from "../models/trips";

@Injectable({ providedIn: 'root' })
export class TripsService {

  private readonly apiUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/trips";
  }

  add(trip: TripCreateDTO, formData: FormData): Observable<TripCreateDTO> {
    trip.ImagesAsFiles?.forEach(file => formData.append("ImagesAsFiles", file, file.name));
    formData.set("EndTime", trip.EndTime? trip.EndTime.toISOString() : "");
    formData.set("Distance", trip.Distance.toString());
    formData.set("StartTimeZoneOffset", trip.StartTimeZoneOffset.toString());
    formData.set("FinishTimeZoneOffset", trip.FinishTimeZoneOffset.toString());
    formData.set("DriverId", trip.DriverId ? trip.DriverId!.toString() : "");
    formData.set("RoutePointsAsString", trip.RoutePointsAsString);
    formData.set("Public", trip.Public.toString());
    return this.http.post<TripCreateDTO>(this.apiUrl + '/create', formData);
  }

  delete(tripId: number): Observable<number>{
    return this.http.delete<number>(this.apiUrl + `/delete/${tripId}`);
  }

  getAll(): Observable<TripReadDTO[]>{
    return this.http.get<TripReadDTO[]>(this.apiUrl + '/index');
  }
}
