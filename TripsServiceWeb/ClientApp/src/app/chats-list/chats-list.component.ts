import {Component, OnInit} from '@angular/core';
import {ChatCreateDTO, ChatListDTO} from "../../models/chats";
import {ChatsService} from "../../services/chats.service";
import {AccountService} from "../../services/account.service";

@Component({
  selector: 'app-chats-list',
  templateUrl: './chats-list.component.html'
})
export class ChatsListComponent implements OnInit {
  chats: ChatListDTO[] = [];
  isUserAdmin: boolean = false;
  chatToAdd: ChatCreateDTO = new ChatCreateDTO();
  validationErrors: {Name?: string[]} = {};

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
    this.accountService.currentUserInfo$.subscribe({
      next: value => this.isUserAdmin = value.role === "Admin"
    });
  }

  create(): void {
    this.chatsService.add(this.chatToAdd).subscribe({
      next: (response) => this.chats.push(response),
      error: (error) => this.validationErrors = error.error.errors || error.error
    });
  }
}
