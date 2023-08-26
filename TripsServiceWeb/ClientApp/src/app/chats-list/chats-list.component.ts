import {Component, OnInit} from '@angular/core';
import {ChatCreateDTO, ChatListDTO} from "../../models/chats";
import {ChatsService} from "../../services/chats.service";
import {AccountService} from "../../services/account.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-chats-list',
  templateUrl: './chats-list.component.html'
})
export class ChatsListComponent implements OnInit {
  chats: ChatListDTO[] = [];
  chatToAdd: ChatCreateDTO = new ChatCreateDTO();
  validationErrors: {Name?: string[]} = {};

  isUserAdmin: boolean = false;
  userInfoSubscription: Subscription;

  constructor(private chatsService: ChatsService, private accountService: AccountService) {
  }

  ngOnInit(): void {
    this.chatsService.getAll().subscribe({
      next: (chats) => {
        this.chats = chats;
      },
      error: () => {
        alert("Impossible to load chats list. Try later");
      }
    });
    this.userInfoSubscription = this.accountService.currentUserInfo$.subscribe((userInfo) => {
      this.isUserAdmin = userInfo.role === "Admin";
    });
  }

  create(): void {
    this.chatsService.add(this.chatToAdd).subscribe({
      next: (response) => this.chats.push(response),
      error: (error) => this.validationErrors = error.error.errors || error.error
    });
  }

  delete(id: number): void {
    this.chatsService.delete(id).subscribe({
      next: () => this.chats = this.chats.filter(chat => chat.id !== id),
      error: (error) => alert(error.error)
    })
  }
}
