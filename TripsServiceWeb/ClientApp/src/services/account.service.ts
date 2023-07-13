import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {UserLoginDTO} from "../models/login";
import {UserSignupDTO} from "../models/signup";

export type UserNameResponse = {
  userName: string
}

@Injectable({ providedIn: 'root' })
export class AccountService {

  private apiUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/account";
  }

  signup(user: UserSignupDTO): Observable<Object>{
    return this.http.post(this.apiUrl + '/register', user);
  }

  login(user: UserLoginDTO): Observable<Object>{
    return this.http.post(this.apiUrl + '/login', user);
  }

  logout(): Observable<Object>{
    return this.http.get(this.apiUrl + '/logout');
  }

  getUserName(): Observable<UserNameResponse>{
    return this.http.get<UserNameResponse>(this.apiUrl + '/username');
  }
}
