import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {DriverInfoDTO} from "../models/drivers";

@Injectable({ providedIn: 'root' })
export class DriversService {

  private apiUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/drivers";
  }

  getAll(): Observable<DriverInfoDTO[]> {
    return this.http.get<DriverInfoDTO[]>(this.apiUrl + '/list');
  }
}
