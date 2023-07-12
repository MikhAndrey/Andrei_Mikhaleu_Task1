import {StatisticsService} from "../services/statistics.service";
import {Directive, OnInit} from "@angular/core";

export interface RoutePointsDTO {
  latitude: number,
  longitude: number
}

export interface DurationInMonth {
  month: string,
  totalDuration: number
}

@Directive()
export class YearStatisticsCore implements OnInit {

  selectedYear: number = 0;
  yearsOfTrips: number[] = [];

  constructor(protected statisticsService: StatisticsService) { }

  ngOnInit(): void {
    this.statisticsService.getYears().subscribe(
      (response: any) => {
        this.yearsOfTrips = response;
        const userHasTrips: boolean = this.yearsOfTrips.length > 0;
        document.getElementById("main-content")!.hidden = !userHasTrips;
        document.getElementById("no-content")!.hidden = userHasTrips;
        if (userHasTrips) {
          this.selectedYear = this.yearsOfTrips[0];
          this.onYearChange();
        }
      }
    );
  }

  onYearChange(): void {}
}
