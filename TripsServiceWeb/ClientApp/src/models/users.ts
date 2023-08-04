export class UserListDTO {
  id: number;
  role: string;
  userName: string;
}

export class UserChatDTO extends UserListDTO {
  participationId: number;
}
