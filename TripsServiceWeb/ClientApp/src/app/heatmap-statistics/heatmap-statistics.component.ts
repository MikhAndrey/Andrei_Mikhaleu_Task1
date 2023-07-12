import {Component, Inject, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-heatmap-statistics',
  templateUrl: './heatmap-statistics.component.html',
})
export class HeatmapStatisticsComponent implements OnInit {
  heatmapData: any;
  selectedYear?: number;
  yearsOfTrips?: number[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit(): void {
    this.http.get(this.baseUrl + 'api/statistics/years').subscribe(
      (response: any) => {
        this.yearsOfTrips = response;
        if (this.yearsOfTrips)
          this.selectedYear = this.yearsOfTrips[0];
      }
    );
  }

  onYearChange(): void {

  }

  initializeMap(): void {

  }

}
