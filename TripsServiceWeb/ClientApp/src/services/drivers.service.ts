﻿import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {DriverDetailsDTO, DriverInfoDTO} from "../models/drivers";

@Injectable({ providedIn: 'root' })
export class DriversService {

  private readonly apiUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/drivers";
  }

  getAll(): Observable<DriverInfoDTO[]> {
    return this.http.get<DriverInfoDTO[]>(this.apiUrl + '/list');
  }

  mapLinks(drivers: DriverInfoDTO[]): void{
    drivers.forEach(driver => driver.photoLink = this.baseUrl + driver.photoLink);
  }

  getById(id: number): Observable<DriverDetailsDTO> {
    return this.http.get<DriverDetailsDTO>(this.apiUrl + `/details/${id}`);
  }
}
