export class CommentCreateDTO {
  TripId: number;
  Message: string;
}

export interface CommentCreateValidationErrors {
  Message?: string[];
}

export class CommentDTO {
  id: number;
  date: Date;
  message: string;
  userName: string;
  timeAgoAsString: string;
}
