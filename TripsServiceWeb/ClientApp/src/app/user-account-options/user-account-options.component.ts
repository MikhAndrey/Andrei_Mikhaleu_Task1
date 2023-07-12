import {Component, Inject, Injectable, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
@Component({
  selector: 'app-user-account-options',
  template: `
    <li *ngIf="!userName" class="nav-item" [routerLinkActive]="['link-active']">
      <a class="nav-link text-dark" [routerLink]="['/register']">Sign up</a>
    </li>
    <li *ngIf="!userName" class="nav-item" [routerLinkActive]="['link-active']">
      <a class="nav-link text-dark" [routerLink]="['/login']">Login</a>
    </li>
    <li *ngIf="userName" class="nav-item">
        <div>Welcome back, {{ userName }}</div>
    </li>
    <li *ngIf="userName" class="nav-item">
        <a class="nav-link text-dark" (click)="logout()">Log out</a>
    </li>
  `
})
export class UserAccountOptionsComponent implements OnInit {
  userName?: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit(): void {
    this.http.get(this.baseUrl + 'api/account/username').subscribe(
      (response: any) => {
        this.userName = response.userName
      }
    );
  }

  logout(): void {
    this.http.get(this.baseUrl + 'api/account/logout').subscribe(
      () => {
        this.userName = undefined;
      }
    );
  }

}
