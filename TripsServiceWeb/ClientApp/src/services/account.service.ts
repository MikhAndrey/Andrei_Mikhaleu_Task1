import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {BehaviorSubject, Observable} from "rxjs";
import {UserLoginDTO} from "../models/login";
import {UserSignupDTO} from "../models/signup";
import {RedirectService} from "./redirect.service";

export type UserNameResponse = {
  userName?: string,
  role?: string
}

@Injectable({ providedIn: 'root' })
export class AccountService {

  private readonly apiUrl: string;

  currentUserInfo$: BehaviorSubject<UserNameResponse> = new BehaviorSubject<UserNameResponse>({});
  public setCurrentUserInfo(userInfo: UserNameResponse) {
    this.currentUserInfo$.next(userInfo);
  }

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private redirectService: RedirectService)
  {
    this.apiUrl = baseUrl + "api/account";
    if (Object.keys(this.currentUserInfo$.getValue()).length === 0)
      this.getUserInfo();
  }

  signup(user: UserSignupDTO): Observable<Object>{
    return this.http.post(this.apiUrl + '/register', user);
  }

  login(user: UserLoginDTO): Observable<Object>{
    return this.http.post(this.apiUrl + '/login', user);
  }

  logout(): void{
    this.http.get(this.apiUrl + '/logout').subscribe({
      next: () => {
        this.setCurrentUserInfo({userName: undefined, role: undefined});
        this.redirectService.redirectToAddress("");
      }
    });
  }

  getUserInfo(): void{
    this.http.get<UserNameResponse>(this.apiUrl + '/userinfo').subscribe({
      next: value => {
        this.setCurrentUserInfo(value);
      }
    });
  }
}
