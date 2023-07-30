import {RoutePointDTO} from "./routePoints";
import {CommentDTO} from "./comments";
import {ImageDTO} from "./images";
import {DriverInfoDTO} from "./drivers";

export class TripCreateDTO {
  startTime: Date;
  endTime?: Date;
  distance: number;
  startTimeZoneOffset: number;
  finishTimeZoneOffset: number;
  driverId?: number;
  routePointsAsString: string;
  imagesAsFiles?: File[];
  name: string;
  public: boolean;
  description?: string;

  constructor() {
    this.distance = 0;
    this.startTimeZoneOffset = 0;
    this.finishTimeZoneOffset = 0;
    this.routePointsAsString = "";
    this.name = "";
    this.public = false;
    this.imagesAsFiles = [];
  }
}

export class AdminTripCreateDTO extends TripCreateDTO {
  userId: number;
  constructor() {
    super();
  }
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

  constructor() {
    super();
    this.images = [];
    this.comments = [];
    this.routePoints = [];
  }
}

export class TripDateChangesDTO {
  newStartTimeAsString: string;
  newFinishTimeAsString: string;
  newTimeInfo: string;
}

export class TripEditDTO extends TripCreateDTO {
  routePoints: RoutePointDTO[];
  images: ImageDTO[];
  id: number;
  constructor() {
    super();
    this.routePoints = [];
    this.images = [];
  }
}

export class AdminTripEditDTO extends TripEditDTO {
  userId: number;
  constructor() {
    super();
  }
}

export class PastTripEditDTO {
  images: ImageDTO[];
  imagesAsFiles?: File[];
  name: string;
  public: boolean;
  description?: string;
  id: number;
  constructor() {
    this.imagesAsFiles = [];
    this.images = [];
  }
}

export interface PastTripEditValidationErrors {
  Name?: string[];
}
