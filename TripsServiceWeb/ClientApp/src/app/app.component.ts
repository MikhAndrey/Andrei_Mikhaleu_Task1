import {Component, OnDestroy, OnInit} from '@angular/core';
import {NotificationsService} from "../services/notifications.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit, OnDestroy{
  title = 'app';

  constructor(private notificationsService: NotificationsService) {
  }

  async ngOnInit(): Promise<void> {
    await this.notificationsService.startConnection();
    this.notificationsService.initUserNotifications();
  }

  async ngOnDestroy(): Promise<void> {
    await this.notificationsService.closeConnection();
  }
}
