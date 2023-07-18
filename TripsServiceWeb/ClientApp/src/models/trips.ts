import {RoutePointDTO} from "./routePoints";
import {CommentDTO} from "./comments";
import {ImageDTO} from "./images";
import {DriverInfoDTO} from "./drivers";

export class TripCreateDTO {
  StartTime: Date;
  EndTime?: Date;
  Distance: number;
  StartTimeZoneOffset: number;
  FinishTimeZoneOffset: number;
  DriverId?: number;
  RoutePointsAsString: string;
  ImagesAsFiles?: File[];
  Name: string;
  Public: boolean;
  Description?: string;
}

export interface TripCreateValidationErrors {
  Name?: string[];
  StartTime?: string[];
  EndTime?: string[];
}

export class TripReadDTO {
  id: number;
  name: string;
  description?: string;
  startTime: Date;
  endTime: Date;
  isCurrent: boolean;
  isFuture: boolean;
  isPast: boolean;
  utcStartTimeZone: string;
  utcFinishTimeZone: string;
  rating?: number;
  isNeedToBeRated: boolean;
}

export class TripReadDTOExtended extends TripReadDTO {
  userName: string;
}

export class TripDetailsDTO extends TripReadDTO {
  userId: number;
  isCurrentUserTrip: boolean;
  public: boolean;
  images: ImageDTO[];
  routePoints: RoutePointDTO[];
  comments: CommentDTO[];
  startTimeZoneOffset: number;
  finishTimeZoneOffset: number;
  duration: string;
  distance: number;
  driver?: DriverInfoDTO;
  timeInfo: string;
  feedbackText?: string;
  feedbackId: number;
}

export class TripDateChangesDTO {
  newStartTimeAsString: string;
  newFinishTimeAsString: string;
  newDurationAsString: string;
}
