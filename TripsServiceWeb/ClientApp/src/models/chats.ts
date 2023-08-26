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
  user: UserChatDTO;
}

export class ChatDetailsDTO {
  id: number;
  name: string;
  users: UserChatDTO[];
  messages: ChatMessageDTO[];
  isCurrentUserInChat: boolean;
}

export class ChatSendMessageDTO {
  chatId: number;
  user: UserChatDTO;
  text: string;
}

export class ChatNotificationMessageDTO {
  id?: number;
  chatId: number;
  text: string;
  chatName: string;
  user: UserChatDTO | undefined;
}

export class ChatLeaveDTO {
  chatId: number;
  participationId: number;
}

export class ChatJoinDTO {
  message: ChatMessageDTO;
  user: UserChatDTO;
}
