import {Component, OnDestroy, OnInit} from '@angular/core';
import {ChatsService} from "../../services/chats.service";
import {ChatDetailsDTO, ChatMessageDTO} from "../../models/chats";
import {ActivatedRoute} from "@angular/router";
import {ChatWebsocketService} from "../../services/chatWebsocket.service";

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html'
})
export class ChatComponent implements OnInit, OnDestroy {
  chat: ChatDetailsDTO = new ChatDetailsDTO();
  messageText: string;

  constructor(
    private chatsService: ChatsService,
    private route: ActivatedRoute,
    private chatWebsocketService: ChatWebsocketService) {
  }

  ngOnInit(): void {
    const id = parseInt(this.route.snapshot.paramMap.get('id')!);
    this.chatsService.getById(id).subscribe({
      next: (chat) => {
        this.chat = chat;
      },
      error: () => {
        alert("Impossible to load chat data. Try later");
      }
    });
    this.chatWebsocketService.onReceiveMessage = this.messageReceiveHandler.bind(this);
    this.chatWebsocketService.startConnection();
  }

  ngOnDestroy() {
    this.chatWebsocketService.closeConnection();
  }

  joinChat(): void {
    this.chatsService.addUserToChat(this.chat.id).subscribe({
      next: () => this.chat.isCurrentUserInChat = true,
      error: (error) => alert(error.error)
    })
  }

  messageReceiveHandler(message: ChatMessageDTO) {
    this.chat.messages.push(message);
  }
}
