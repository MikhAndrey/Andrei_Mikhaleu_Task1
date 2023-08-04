import {AfterViewChecked, Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {ChatsService} from "../../services/chats.service";
import {ChatDetailsDTO, ChatMessageDTO, ChatNotificationMessageDTO, ChatSendMessageDTO} from "../../models/chats";
import {ActivatedRoute} from "@angular/router";
import {ChatWebsocketService} from "../../services/chatWebsocket.service";
import {NotificationsService} from "../../services/notifications.service";

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy, AfterViewChecked {
  chat: ChatDetailsDTO = new ChatDetailsDTO();
  messageToSend: ChatSendMessageDTO = new ChatSendMessageDTO();
  private userIdsToNotify: string[];

  @ViewChild('messagesContainer') messagesContainer: ElementRef;

  constructor(
    private chatsService: ChatsService,
    private route: ActivatedRoute,
    private chatWebsocketService: ChatWebsocketService,
    private notificationsService: NotificationsService) {
  }

  async ngOnInit(): Promise<void> {
    const chatId: number = parseInt(this.route.snapshot.paramMap.get('id')!);
    this.chatsService.getById(chatId).subscribe({
      next: (chat) => {
        this.chat = chat;
        this.messageToSend.chatId = this.chat.id;
        this.userIdsToNotify = this.chat.users.reduce((acc, elem) => {
          if (elem !== null && elem.role === "Admin")
            acc.push(elem.id.toString())
          return acc;
        }, new Array<string>());
      },
      error: () => {
        alert("Impossible to load chat data. Try later");
      }
    });

    this.setCurrentChatParticipationId(chatId);

    this.chatWebsocketService.onReceiveMessage = this.messageReceiveHandler.bind(this);
    await this.chatWebsocketService.startConnection();
    await this.chatWebsocketService.addUserToChat(chatId);
  }

  async ngOnDestroy() {
    await this.chatWebsocketService.removeUserFromChat(this.chat.id);
    this.chatWebsocketService.closeConnection();
  }

  ngAfterViewChecked() {
    this.scrollDownMessagesContainer();
  }

  joinChat(): void {
    this.chatsService.addUserToChat(this.chat.id).subscribe({
      next: (participationId: number) => {
        this.messageToSend.chatParticipationId = participationId;
        this.chat.isCurrentUserInChat = true;
      },
      error: err => alert(err.error)
    });
  }

  async sendMessage(): Promise<void> {
    this.chatsService.sendMessage(this.messageToSend).subscribe({
      next: () => this.messageToSend.text = "",
      error: err => alert(err.error)
    });

    const messageToNotify: ChatNotificationMessageDTO = {
      chatName: this.chat.name,
      chatId: this.chat.id,
      text: this.messageToSend.text
    };

    await this.notificationsService.broadcastChatNotification(this.userIdsToNotify, messageToNotify);
  }

  leaveChat(): void {
    this.chatsService.leaveChat(this.chat.id, this.messageToSend.chatParticipationId).subscribe({
      next: () => this.chat.isCurrentUserInChat = false
    });
  }

  private setCurrentChatParticipationId(chatId: number): void {
    this.chatsService.getParticipationId(chatId).subscribe({
      next: (response) => this.messageToSend.chatParticipationId = response,
      error: () => console.log("Current user is not a chat member")
    });
  }

  private messageReceiveHandler(message: ChatMessageDTO) {
    this.chat.messages.push(message);
  }

  private scrollDownMessagesContainer(){
    this.messagesContainer.nativeElement.scrollTo({
      top: this.messagesContainer.nativeElement.scrollHeight,
      behavior: 'smooth'
    });
  }
}
