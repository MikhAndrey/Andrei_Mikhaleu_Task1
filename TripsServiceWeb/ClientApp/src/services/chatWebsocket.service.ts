import {Injectable} from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {HttpTransportType} from "@microsoft/signalr";
import {environment} from "../environments/environment";
import {ChatMessageDTO} from "../models/chats";

@Injectable({
  providedIn: 'root'
})
export class ChatWebsocketService {
  private hubConnection: signalR.HubConnection;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.BASE_API_URL + "chathub", {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .build();
  }

  async startConnection() {
    await this.hubConnection.start();
  }

  set onReceiveMessage(callback: (message: ChatMessageDTO) => void){
    this.hubConnection.on('BroadcastMessage', callback);
  }

  set onNotificationReceiverJoining(callback: (userId: number) => void){
    this.hubConnection.on('BroadcastJoiningUserId', callback);
  }

  set onNotificationReceiverLeaving(callback: (userId: number) => void){
    this.hubConnection.on('BroadcastLeavingUserId', callback);
  }

  async addUserToChat(chatId: number) {
    await this.hubConnection.invoke("AddUserToGroup", chatId);
  }

  async removeUserFromChat(chatId: number) {
    await this.hubConnection.invoke("RemoveUserFromGroup", chatId);
  }

  async addUserToNotificationReceivers(chatId: number, userId: number) {
    await this.hubConnection.invoke("AddUserToNotificationReceivers", chatId, userId);
  }

  async removeUserFromNotificationReceivers(chatId: number, userId: number) {
    await this.hubConnection.invoke("RemoveUserFromNotificationReceivers", chatId, userId);
  }

  closeConnection(){
    this.hubConnection
      .stop()
      .catch(err => console.log('Error while closing connection: ' + err));
  }
}
