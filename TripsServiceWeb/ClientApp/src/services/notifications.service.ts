import {Injectable} from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {HttpTransportType} from "@microsoft/signalr";
import {environment} from "../environments/environment";
import {ChatNotificationMessageDTO} from "../models/chats";
import {BehaviorSubject, Subscription} from "rxjs";
import {AccountService} from "./account.service";

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {
  private hubConnection: signalR.HubConnection;

  notifications$: BehaviorSubject<ChatNotificationMessageDTO[]> = new BehaviorSubject<ChatNotificationMessageDTO[]>([]);
  notificationsInitSubscription: Subscription;

  constructor(private accountService: AccountService) {
    this.notificationsInitSubscription = this.accountService.currentUserInfo$.subscribe({
      next: value => {
        if (Object.keys(value).length > 0)
          this.initUserNotifications.bind(this);
      }
    });
  }

  async startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.BASE_API_URL + "notificationhub", {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .build();
    this.hubConnection.on("BroadcastChatNotification", this.addNotification.bind(this));
    await this.hubConnection.start();
  }

  initUserNotifications(): void {
    this.accountService.getUserNotifications().subscribe({
      next: value => this.notifications$.next(value),
      error: err => console.log(err)
    });
    this.notificationsInitSubscription.unsubscribe();
  }

  closeConnection(): void {
    this.hubConnection
      .stop()
      .catch(err => console.log('Error while closing connection: ' + err));
  }

  async broadcastChatNotification(receivers: string[], message: ChatNotificationMessageDTO): Promise<void> {
    await this.hubConnection.invoke("Notify", receivers, message);
  }

  addNotification(message: ChatNotificationMessageDTO): void {
    const updatedNotifications = [...this.notifications$.value, message];
    this.notifications$.next(updatedNotifications);
  }
}
