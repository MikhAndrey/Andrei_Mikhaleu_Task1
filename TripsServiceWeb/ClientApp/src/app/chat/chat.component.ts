import {Component, OnInit} from '@angular/core';
import {ChatsService} from "../../services/chats.service";
import {ChatDetailsDTO} from "../../models/chats";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html'
})
export class ChatComponent implements OnInit {
  chat: ChatDetailsDTO;

  constructor(private chatsService: ChatsService, private route: ActivatedRoute) {
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
  }
}
