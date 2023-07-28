﻿import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {UserListDTO} from "../models/users";

@Injectable({ providedIn: 'root' })
export class UsersService {

  private readonly apiUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/users";
  }

  getAll(): Observable<UserListDTO[]> {
    return this.http.get<UserListDTO[]>(this.apiUrl + '/index');
  }
}