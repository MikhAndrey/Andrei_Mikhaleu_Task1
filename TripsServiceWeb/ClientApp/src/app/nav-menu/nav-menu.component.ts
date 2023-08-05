import {Component, OnDestroy, OnInit} from '@angular/core';
import {AccountService, UserNameResponse} from "../../services/account.service";
import {Subscription} from "rxjs";
import {NotificationsService} from "../../services/notifications.service";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit, OnDestroy{

  userInfo: UserNameResponse = {};
  userInfoSubscription: Subscription;

  notificationsCount: number = 0;
  userNotificationsCountSubscription: Subscription;
  initialNotificationsSubscription: Subscription;

  constructor(private accountService: AccountService, private notificationsService: NotificationsService) {
  }

  ngOnInit(): void {
    this.userInfoSubscription = this.accountService.currentUserInfo$.subscribe((userInfo) => {
      this.userInfo = userInfo
    });
    this.userNotificationsCountSubscription = this.notificationsService.incomingNotification$.subscribe((notification) => {
     if (notification) {
       this.notificationsCount++;
     }
    });
    this.initialNotificationsSubscription = this.notificationsService.initialNotifications$.subscribe((notifications) => {
      this.notificationsCount = notifications.length;
    });
  }

  logout(): void {
    this.accountService.logout();
  }

  ngOnDestroy() {
    this.userInfoSubscription.unsubscribe();
    this.userNotificationsCountSubscription.unsubscribe();
    this.initialNotificationsSubscription.unsubscribe();
  }
}
