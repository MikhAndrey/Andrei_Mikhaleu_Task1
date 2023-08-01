import {UserChatDTO} from "./users";

export class ChatListDTO {
  id: number;
  name: string;
}

export class ChatCreateDTO {
  name: string;
}

export class ChatMessageDTO {
  id: number;
  text: string;
  participationId: number;
}

export class ChatDetailsDTO {
  name: string;
  users: UserChatDTO[];
}
