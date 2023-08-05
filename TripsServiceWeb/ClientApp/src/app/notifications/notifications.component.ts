﻿import {Component, OnInit} from '@angular/core';
import {NotificationsService} from "../../services/notifications.service";
import {Subscription} from "rxjs";
import {ChatNotificationMessageDTO} from "../../models/chats";
import {Router} from "@angular/router";

type NotificationDisplayOptions = {
  displayDuration: number,
  maxMessagesCount: number
}

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit{

  incomingNotificationSubscription: Subscription;
  notifications: ChatNotificationMessageDTO[] = [];
  notificationDisplayOptions: NotificationDisplayOptions = {
    displayDuration: 5000,
    maxMessagesCount: 3
  }

  timeoutIds: number[] = [];

  constructor(private notificationsService: NotificationsService, private router: Router) {
  }

  ngOnInit() {
    this.incomingNotificationSubscription = this.notificationsService.incomingNotification$.subscribe({
      next: value => {
        if (value) {
          if (this.notifications.length === this.notificationDisplayOptions.maxMessagesCount) {
            this.shiftNotificationsQueue();
          }
          this.notifications.push(value);
          const timeoutId = setTimeout(() => {
            this.removeFromNotificationsQueue(value)
          }, this.notificationDisplayOptions.displayDuration);
          this.timeoutIds.push(timeoutId)
        }
      }
    });
  }

  private shiftNotificationsQueue(){
    this.notifications.shift();
    clearTimeout(this.timeoutIds[0]);
    this.timeoutIds.shift();
  }

  private removeFromNotificationsQueue(notification: ChatNotificationMessageDTO) {
    const notificationToRemoveIndex = this.notifications.indexOf(notification);
    if (notificationToRemoveIndex !== -1) {
      this.shiftNotificationsQueue()
    }
  }

  async redirectToChat(chatId: number){
    await this.router.navigate([`chats/${chatId}`]);
  }
}
