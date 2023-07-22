export class FeedbackCreateDTO {
  TripId: number;
  Text?: string;
  Rating: number;
}

export class FeedbackReadDTO {
  id: number;
  text?: string;
  rating: number;
  userName?: string;
}
