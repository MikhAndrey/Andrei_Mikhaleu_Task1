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

  constructor() {
    this.StartTime = new Date();
    this.Distance = 0;
    this.StartTimeZoneOffset = 0;
    this.FinishTimeZoneOffset = 0;
    this.RoutePointsAsString = '';
    this.Name = '';
    this.Public = false;
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

  constructor() {
    this.id = 0;
    this.name = "";
    this.startTime = new Date();
    this.endTime = new Date();
    this.isCurrent = false;
    this.isFuture = false;
    this.isPast = false;
    this.utcStartTimeZone = "";
    this.utcFinishTimeZone = "";
    this.isNeedToBeRated = false;
  }
}
