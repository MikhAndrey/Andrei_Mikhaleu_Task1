import {Component, OnDestroy, OnInit} from '@angular/core';
import {ChatsService} from "../../services/chats.service";
import {ChatDetailsDTO, ChatMessageDTO, ChatSendMessageDTO} from "../../models/chats";
import {ActivatedRoute} from "@angular/router";
import {ChatWebsocketService} from "../../services/chatWebsocket.service";

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy {
  chat: ChatDetailsDTO = new ChatDetailsDTO();
  messageToSend: ChatSendMessageDTO = new ChatSendMessageDTO();

  constructor(
    private chatsService: ChatsService,
    private route: ActivatedRoute,
    private chatWebsocketService: ChatWebsocketService) {
  }

  ngOnInit(): void {
    const chatId: number = parseInt(this.route.snapshot.paramMap.get('id')!);
    this.chatsService.getById(chatId).subscribe({
      next: (chat) => {
        this.chat = chat;
      },
      error: () => {
        alert("Impossible to load chat data. Try later");
      }
    });

    this.setCurrentChatParticipationId(chatId);

    this.chatWebsocketService.onReceiveMessage = this.messageReceiveHandler.bind(this);
    this.chatWebsocketService.startConnection();
  }

  ngOnDestroy() {
    this.chatWebsocketService.closeConnection();
  }

  joinChat(): void {
    this.chatsService.addUserToChat(this.chat.id).subscribe({
      next: (participationId: number) => {
        this.messageToSend.chatParticipationId = participationId;
        this.chat.isCurrentUserInChat = true;
      },
      error: err => alert(err.error)
    })
  }

  sendMessage(): void {
    this.chatsService.sendMessage(this.messageToSend).subscribe({
      next: () => this.messageToSend.text = "",
      error: err => alert(err.error)
    })
  }

  setCurrentChatParticipationId(chatId: number): void {
    this.chatsService.getParticipationId(chatId).subscribe({
      next: (response) => this.messageToSend.chatParticipationId = response
    });
  }

  messageReceiveHandler(message: ChatMessageDTO) {
    this.chat.messages.push(message);
  }
}
