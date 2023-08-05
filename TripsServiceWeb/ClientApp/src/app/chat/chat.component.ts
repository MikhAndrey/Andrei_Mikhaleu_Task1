import {AfterViewChecked, Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {ChatsService} from "../../services/chats.service";
import {
  ChatDetailsDTO,
  ChatJoinDTO,
  ChatMessageDTO,
  ChatNotificationMessageDTO,
  ChatSendMessageDTO
} from "../../models/chats";
import {ActivatedRoute} from "@angular/router";
import {ChatWebsocketService} from "../../services/chatWebsocket.service";
import {NotificationsService} from "../../services/notifications.service";
import {AccountService} from "../../services/account.service";
import {UserChatDTO} from "../../models/users";

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
    private notificationsService: NotificationsService,
    private accountService: AccountService) {
  }

  async ngOnInit(): Promise<void> {
    const chatId: number = parseInt(this.route.snapshot.paramMap.get('id')!);
    this.chatsService.getById(chatId).subscribe({
      next: (chat) => {
        this.chat = chat;
        this.messageToSend.chatId = this.chat.id;

        const currentUser: UserChatDTO | undefined = this.chat.users.find(el => el.id === this.accountService.currentUserInfo$.getValue().id);
        if (currentUser)
          this.messageToSend.user = currentUser;

        this.userIdsToNotify = this.chat.users.reduce((acc, elem) => {
          if (elem.role === "Admin" && this.messageToSend.user && elem.id !== this.messageToSend.user.id)
            acc.push(elem.id.toString())
          return acc;
        }, new Array<string>());
      },
      error: () => {
        alert("Impossible to load chat data. Try later");
      }
    });

    this.chatWebsocketService.onReceiveMessage = this.messageReceiveHandler.bind(this);
    this.chatWebsocketService.onNotificationReceiverJoining = this.notificationReceiverJoiningHandler.bind(this);
    this.chatWebsocketService.onNotificationReceiverLeaving = this.notificationReceiverLeavingHandler.bind(this);

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
      next: async(chatDTO: ChatJoinDTO) => {
        this.messageToSend.user = chatDTO.user;
        this.chat.isCurrentUserInChat = true;

        const messageToNotify: ChatNotificationMessageDTO = {
          chatName: this.chat.name,
          chatId: this.chat.id,
          text: chatDTO.message.text,
          user: undefined
        };

        await this.notificationsService.broadcastChatNotification(this.userIdsToNotify, messageToNotify);
        if (this.messageToSend.user.role === "Admin")
          await this.chatWebsocketService.addUserToNotificationReceivers(this.chat.id, this.messageToSend.user.id);
      },
      error: err => alert(err.error)
    });
  }

  async sendMessage(): Promise<void> {
    this.chatsService.sendMessage(this.messageToSend).subscribe({
      next: async() => {
        const messageToNotify: ChatNotificationMessageDTO = {
          chatName: this.chat.name,
          chatId: this.chat.id,
          text: this.messageToSend.text,
          user: this.messageToSend.user
        };
        this.messageToSend.text = "";

        await this.notificationsService.broadcastChatNotification(this.userIdsToNotify, messageToNotify);
      },
      error: err => alert(err.error)
    });
  }

  leaveChat(): void {
    this.chatsService.leaveChat(this.chat.id, this.messageToSend.user.participationId).subscribe({
      next: async (message: ChatMessageDTO) => {
        this.chat.isCurrentUserInChat = false

        const messageToNotify: ChatNotificationMessageDTO = {
          chatName: this.chat.name,
          chatId: this.chat.id,
          text: message.text,
          user: undefined
        };

        await this.notificationsService.broadcastChatNotification(this.userIdsToNotify, messageToNotify);
        if (this.messageToSend.user.role === "Admin")
          await this.chatWebsocketService.removeUserFromNotificationReceivers(this.chat.id, this.messageToSend.user.id);
      }
    });
  }

  private messageReceiveHandler(message: ChatMessageDTO) {
    this.chat.messages.push(message);
  }

  private notificationReceiverJoiningHandler(userId: number){
    if (this.messageToSend.user && userId !== this.messageToSend.user.id)
      this.userIdsToNotify.push(userId.toString());
  }

  private notificationReceiverLeavingHandler(userId: number){
    this.userIdsToNotify = this.userIdsToNotify.filter(el => el !== userId.toString());
  }

  private scrollDownMessagesContainer(){
    this.messagesContainer.nativeElement.scrollTo({
      top: this.messagesContainer.nativeElement.scrollHeight,
      behavior: 'smooth'
    });
  }
}
