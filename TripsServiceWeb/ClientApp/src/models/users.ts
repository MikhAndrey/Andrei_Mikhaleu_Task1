import {ChatMessageDTO} from "./chats";

export class UserListDTO {
  id: number;
  role: string;
  userName: string;
}

export class UserChatDTO extends UserListDTO {
  chatMessages: ChatMessageDTO[];
}
