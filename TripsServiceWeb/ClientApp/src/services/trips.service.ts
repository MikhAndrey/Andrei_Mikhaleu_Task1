import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {TripCreateDTO} from "../models/trips";

@Injectable({ providedIn: 'root' })
export class TripsService {

  private apiUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/trips";
  }

  add(trip: TripCreateDTO): Observable<TripCreateDTO> {
    return this.http.post<TripCreateDTO>(this.apiUrl + '/create', trip);
  }
}
