﻿import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {
  TripCreateDTO,
  TripDateChangesDTO,
  TripDetailsDTO,
  TripEditDTO,
  TripReadDTO,
  TripReadDTOExtended
} from "../../models/trips";

@Injectable({ providedIn: 'root' })
export class TripsService {

  private readonly apiUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/trips";
  }

  add(trip: TripCreateDTO, formData: FormData): Observable<TripCreateDTO> {
    trip.imagesAsFiles?.forEach(file => formData.append("ImagesAsFiles", file, file.name));
    if (trip.endTime) {
      trip.endTime.setTime(trip.endTime.getTime() - trip.endTime.getTimezoneOffset() * 60 * 1000);
      formData.set("EndTime", new Date(trip.endTime).toISOString());
    }
    formData.set("Distance", trip.distance.toLocaleString());
    formData.set("StartTimeZoneOffset", trip.startTimeZoneOffset.toString());
    formData.set("FinishTimeZoneOffset", trip.finishTimeZoneOffset.toString());
    formData.set("DriverId", trip.driverId ? trip.driverId!.toString() : "");
    formData.set("RoutePointsAsString", trip.routePointsAsString);
    formData.set("Public", trip.public.toString());
    return this.http.post<TripCreateDTO>(this.apiUrl + '/create', formData);
  }

  delete(tripId: number): Observable<number>{
    return this.http.delete<number>(this.apiUrl + `/delete/${tripId}`);
  }

  getAll(): Observable<TripReadDTO[]>{
    return this.http.get<TripReadDTO[]>(this.apiUrl + '/index');
  }

  getActivity(): Observable<TripReadDTOExtended[]>{
    return this.http.get<TripReadDTOExtended[]>(this.apiUrl + '/public');
  }

  getHistory(): Observable<TripReadDTO[]>{
    return this.http.get<TripReadDTO[]>(this.apiUrl + '/history');
  }

  getDetails(tripId: number): Observable<TripDetailsDTO> {
    return this.http.get<TripDetailsDTO>(this.apiUrl + `/details/${tripId}`);
  }

  startTrip(tripId: number): Observable<TripDateChangesDTO> {
    return this.http.post<TripDateChangesDTO>(this.apiUrl + `/start/${tripId}`, tripId);
  }

  finishTrip(tripId: number): Observable<TripDateChangesDTO> {
    return this.http.post<TripDateChangesDTO>(this.apiUrl + `/finish/${tripId}`, tripId);
  }

  updateTripDateInfo(trip: TripDetailsDTO, dateInfo: TripDateChangesDTO): void {
    trip.utcStartTimeZone = dateInfo.newStartTimeAsString;
    trip.utcFinishTimeZone = dateInfo.newFinishTimeAsString;
    trip.duration = dateInfo.newDurationAsString;
  }

  getTripForCurrentEditing(id: number): Observable<TripEditDTO> {
    return this.http.get<TripEditDTO>(this.apiUrl + `/edit/current/${id}`);
  }
}
