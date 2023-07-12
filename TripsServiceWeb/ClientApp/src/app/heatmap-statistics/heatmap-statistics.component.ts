/// <reference types="@types/google.maps" />
import {Component, Inject, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {RoutePointsDTO} from "../../models/routePoints";
import {HeatmapData} from "@angular/google-maps";

@Component({
  selector: 'app-heatmap-statistics',
  templateUrl: './heatmap-statistics.component.html',
})
export class HeatmapStatisticsComponent implements OnInit {
  heatmapData: HeatmapData = [];
  heatMapOptions = {
    radius: 40
  };
  selectedYear?: number;
  yearsOfTrips: number[] = [];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit(): void {
    this.http.get(this.baseUrl + 'api/statistics/years').subscribe(
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

  onYearChange(): void {
    this.http.get<RoutePointsDTO[]>(this.baseUrl + 'api/statistics/heatmap?year=' + this.selectedYear).subscribe(
      (response) => {
        this.heatmapData = response.map(el => {
          return {lat: el.latitude, lng: el.longitude};
        });
      }
    );
  }

}
