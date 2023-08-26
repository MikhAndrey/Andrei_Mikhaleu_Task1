import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {
  AdminTripCreateDTO, AdminTripEditDTO,
  PastTripEditDTO,
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
  private readonly adminApiUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/trips";
    this.adminApiUrl = baseUrl + "api/admin/trips";
  }

  add(trip: TripCreateDTO, formData: FormData): Observable<TripCreateDTO> {
    this.initFormDataForTrip(trip, formData);
    return this.http.post<TripCreateDTO>(this.apiUrl + '/create', formData);
  }

  adminAdd(trip: AdminTripCreateDTO, formData: FormData): Observable<AdminTripCreateDTO> {
    this.initFormDataForTrip(trip, formData);
    formData.set("UserId", trip.userId.toString());
    return this.http.post<AdminTripCreateDTO>(this.adminApiUrl + '/create', formData);
  }

  editCurrent(trip: TripEditDTO, formData: FormData): Observable<TripEditDTO> {
    this.initFormDataForTrip(trip, formData);
    formData.set("Id", trip.id.toString());
    return this.http.put<TripEditDTO>(this.apiUrl + `/edit/current/${trip.id}`, formData);
  }

  adminEdit(trip: AdminTripEditDTO, formData: FormData): Observable<AdminTripEditDTO> {
    this.initFormDataForTrip(trip, formData);
    formData.set("Id", trip.id.toString());
    formData.set("UserId", trip.userId.toString());
    return this.http.put<AdminTripEditDTO>(this.adminApiUrl + `/edit/${trip.id}`, formData);
  }

  editPast(trip: PastTripEditDTO, formData: FormData): Observable<PastTripEditDTO> {
    trip.imagesAsFiles?.forEach(file => formData.append("ImagesAsFiles", file, file.name));
    formData.set("Public", trip.public.toString());
    formData.set("Id", trip.id.toString());
    return this.http.put<PastTripEditDTO>(this.apiUrl + `/edit/past/${trip.id}`, formData);
  }

  delete(tripId: number): Observable<number>{
    return this.http.delete<number>(this.apiUrl + `/delete/${tripId}`);
  }

  getAll(): Observable<TripReadDTO[]>{
    return this.http.get<TripReadDTO[]>(this.apiUrl + '/index');
  }

  getAllUsersTrips(): Observable<TripReadDTOExtended[]>{
    return this.http.get<TripReadDTOExtended[]>(this.adminApiUrl + '/index');
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
    trip.timeInfo = dateInfo.newTimeInfo;
  }

  getTripForCurrentEditing(id: number): Observable<TripEditDTO> {
    return this.http.get<TripEditDTO>(this.apiUrl + `/edit/current/${id}`);
  }

  getTripForAdminEditing(id: number): Observable<AdminTripEditDTO> {
    return this.http.get<AdminTripEditDTO>(this.adminApiUrl + `/edit/${id}`);
  }

  getTripForPastEditing(id: number): Observable<PastTripEditDTO> {
    return this.http.get<PastTripEditDTO>(this.apiUrl + `/edit/past/${id}`);
  }

  private initFormDataForTrip(trip: TripCreateDTO, formData: FormData){
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
  }
}
