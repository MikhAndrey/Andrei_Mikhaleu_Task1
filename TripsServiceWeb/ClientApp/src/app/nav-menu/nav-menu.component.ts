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

  notificationsListExpanded: boolean = false;

  constructor(private accountService: AccountService, public notificationsService: NotificationsService) {
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

  toggleNotificationsExpanded() {
    this.notificationsListExpanded = !this.notificationsListExpanded;
  }

  deleteNotification(event: MouseEvent, id: number){
    event.stopPropagation();
    this.notificationsService.deleteNotification(id);
  }
}
