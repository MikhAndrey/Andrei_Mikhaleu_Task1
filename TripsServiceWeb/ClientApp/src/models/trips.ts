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
