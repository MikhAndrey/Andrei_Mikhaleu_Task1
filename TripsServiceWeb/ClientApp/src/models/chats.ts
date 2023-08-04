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
  chatParticipationId: number;
  user: UserListDTO;
}

export class ChatDetailsDTO {
  id: number;
  name: string;
  users: UserListDTO[];
  messages: ChatMessageDTO[];
  isCurrentUserInChat: boolean;
}

export class ChatSendMessageDTO {
  chatId: number;
  chatParticipationId: number;
  text: string;
}

export class ChatNotificationMessageDTO {
  chatId: number;
  text: string;
  chatName: string;
}

export class ChatLeaveDTO {
  chatId: number;
  participationId: number;
}
