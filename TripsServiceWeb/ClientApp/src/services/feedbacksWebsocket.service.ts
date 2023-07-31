import {Injectable} from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {HttpTransportType} from "@microsoft/signalr";
import {environment} from "../environments/environment";
import {FeedbackReadDTO, FeedbackUpdateDTO} from "../models/feedbacks";

@Injectable({
  providedIn: 'root'
})
export class FeedbacksWebsocketService {
  private hubConnection: signalR.HubConnection;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.BASE_API_URL + "feedbackshub", {
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

  set onDelete(callback: (id: number) => void){
    this.hubConnection.on('FeedbackDelete', callback);
  }

  set onCreate(callback: (feedback: FeedbackReadDTO) => void){
    this.hubConnection.on('FeedbackCreate', callback);
  }

  set onUpdate(callback: (feedback: FeedbackUpdateDTO) => void){
    this.hubConnection.on('FeedbackUpdate', callback);
  }

  closeConnection(){
    this.hubConnection
      .stop()
      .catch(err => console.log('Error while closing connection: ' + err));
  }
}
