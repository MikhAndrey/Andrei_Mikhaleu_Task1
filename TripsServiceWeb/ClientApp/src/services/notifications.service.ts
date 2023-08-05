import {Inject, Injectable} from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {HttpTransportType} from "@microsoft/signalr";
import {environment} from "../environments/environment";
import {ChatNotificationMessageDTO} from "../models/chats";
import {BehaviorSubject, Subscription} from "rxjs";
import {AccountService} from "./account.service";
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {
  private hubConnection: signalR.HubConnection;

  private readonly apiUrl: string;

  notifications: ChatNotificationMessageDTO[] = [];

  incomingNotification$: BehaviorSubject<ChatNotificationMessageDTO | undefined> = new BehaviorSubject<ChatNotificationMessageDTO | undefined>(undefined);
  notificationsInitSubscription: Subscription;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private accountService: AccountService,
    private router: Router
  ) {
    this.apiUrl = baseUrl + "api/notifications";
    this.notificationsInitSubscription = this.accountService.currentUserInfo$.subscribe({
      next: value => {
        if (Object.keys(value).length > 0) {
          this.initUserNotifications();
        }
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
      next: notifications => {
        this.notifications = notifications;
      },
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

  addNotification(notification: ChatNotificationMessageDTO): void {
    this.incomingNotification$.next(notification);
    this.notifications.push(notification);
  }

  deleteNotification(id: number): void {
    this.http.delete(this.apiUrl + `/delete/${id}`).subscribe({
      next: () => this.notifications = this.notifications.filter(el => el.id !== id)
    });
  }

  async deleteAndRedirectToChat(id: number, chatId: number){
    this.deleteNotification(id);
    await this.router.navigate([`chats/${chatId}`]);
  }
}
