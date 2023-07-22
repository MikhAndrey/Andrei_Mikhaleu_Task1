import {Component, Injectable, OnInit} from '@angular/core';
import {AccountService, UserNameResponse} from "../../services/account.service";
import {RedirectService} from "../../services/redirect.service";

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

  constructor(private accountService: AccountService, private redirectionService: RedirectService) {
  }

  ngOnInit(): void {
    this.accountService.getUserName().subscribe(
      (response: UserNameResponse) => {
        this.accountService.isAuthenticated = response.userName !== null
        this.userName = response.userName
      }
    );
  }

  logout(): void {
    this.accountService.logout().subscribe(
      () => {
        this.userName = undefined;
        this.accountService.isAuthenticated = false;
        this.redirectionService.redirectToAddress("");
      }
    );
  }

}
