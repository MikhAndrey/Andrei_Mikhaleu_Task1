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

  startConnection() {
    this.hubConnection
      .start()
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  set onReceiveMessage(callback: (message: ChatMessageDTO) => void){
    this.hubConnection.on('BroadcastMessage', callback);
  }

  closeConnection(){
    this.hubConnection
      .stop()
      .catch(err => console.log('Error while closing connection: ' + err));
  }
}
