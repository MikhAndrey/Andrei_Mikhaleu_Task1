import {Component, Inject, Injectable, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
@Component({
  selector: 'app-user-greetings',
  template: '<div *ngIf="userName">Welcome back, {{ userName }}</div>'
})
export class UserGreetingsComponent implements OnInit {
  userName?: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit(): void {
    this.http.get(this.baseUrl + 'api/account/username').subscribe(
      (response: any) => {
        this.userName = response.userName
      }
    );
  }

}
