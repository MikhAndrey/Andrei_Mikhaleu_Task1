export interface UserSignupDTO {
    UserName?: string,
    Email?: string,
    Password?: string
}

export interface SignupValidationErrors {
  UserName?: string[]
  Email?: string[],
  Password?: string[]
}
