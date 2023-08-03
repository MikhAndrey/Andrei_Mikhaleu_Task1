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

  async addUserToChat(chatId: number) {
    await this.hubConnection.invoke("AddUserToGroup", chatId);
  }

  async removeUserFromChat(chatId: number) {
    await this.hubConnection.invoke("RemoveUserFromGroup", chatId);
  }

  closeConnection(){
    this.hubConnection
      .stop()
      .catch(err => console.log('Error while closing connection: ' + err));
  }
}
