import {ImageDTO} from "./images";
import {FeedbackReadDTO} from "./feedbacks";

export class DriverInfoDTO {
   id: number;
   name: string;
   photoLink?: string;
   experience: number;
   averageRating: number;
}

export class DriverDetailsDTO {
  id: number;
  name: string;
  images: ImageDTO[];
  experience: number;
  averageRating: number;
  feedbacks: FeedbackReadDTO[]

  constructor() {
    this.feedbacks = [];
    this.images = [];
  }
}
