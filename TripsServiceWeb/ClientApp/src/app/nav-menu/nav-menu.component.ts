import {Component, OnDestroy, OnInit} from '@angular/core';
import {AccountService, UserNameResponse} from "../../services/account.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit, OnDestroy{

  userInfo: UserNameResponse = {};
  userInfoSubscription: Subscription;

  constructor(private accountService: AccountService) {
  }

  ngOnInit(): void {
    this.userInfoSubscription = this.accountService.currentUserInfo$.subscribe((userInfo) => {
      this.userInfo = userInfo
    });
  }

  logout(): void {
    this.accountService.logout();
  }

  ngOnDestroy() {
    this.userInfoSubscription.unsubscribe();
  }
}
