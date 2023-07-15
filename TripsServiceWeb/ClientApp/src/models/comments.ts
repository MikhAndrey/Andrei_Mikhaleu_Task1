export class CommentCreateDTO {
  TripId: number;
  Message: string;

  constructor() {
    this.TripId = 0;
    this.Message = "";
  }
}

export interface CommentCreateValidationErrors {
  Message?: string[];
}
