export interface UserLoginDTO {
  UserName?: string,
  Password?: string,
  RememberMe?: boolean
}

export interface LoginValidationErrors {
  UserName?: string[],
  Password?: string[],
  Model?: string[]
}
