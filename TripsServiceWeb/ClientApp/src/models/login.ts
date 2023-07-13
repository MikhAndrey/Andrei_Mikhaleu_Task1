export class UserLoginDTO {
  UserName: string;
  Password: string;
  RememberMe: boolean;

  constructor() {
    this.UserName = '';
    this.Password = ''
    this.RememberMe = false;
  }
}

export interface LoginValidationErrors {
  UserName?: string[],
  Password?: string[],
  Model?: string[]
}
