import {UserListDTO} from "./users";

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
  user: UserListDTO;
}

export class ChatDetailsDTO {
  id: number;
  name: string;
  messages: ChatMessageDTO[];
  isCurrentUserInChat: boolean;
}

export class ChatSendMessageDTO {
  chatParticipationId: number;
  text: string;
}
