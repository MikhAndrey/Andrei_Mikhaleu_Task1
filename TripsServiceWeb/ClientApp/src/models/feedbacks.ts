export class FeedbackCreateDTO {
  TripId: number;
  Text?: string;
  Rating: number;

  constructor() {
    this.TripId = 0;
    this.Rating = 0;
  }
}
