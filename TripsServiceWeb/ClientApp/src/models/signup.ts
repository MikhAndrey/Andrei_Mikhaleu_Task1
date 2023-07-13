export class UserSignupDTO {
    UserName: string;
    Email: string;
    Password: string;

    constructor() {
      this.UserName = '';
      this.Email = '';
      this.Password = '';
    }
}

export interface SignupValidationErrors {
  UserName?: string[]
  Email?: string[],
  Password?: string[]
}
