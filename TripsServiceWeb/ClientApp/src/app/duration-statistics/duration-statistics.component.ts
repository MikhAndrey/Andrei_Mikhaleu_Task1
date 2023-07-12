import {Component, Inject, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {DurationInMonth} from "../../models/statistics";
import {ChartType, Row} from "angular-google-charts";

@Component({
  selector: 'app-duration-statistics',
  templateUrl: './duration-statistics.component.html',
})
export class DurationStatisticsComponent implements OnInit {
  durationData: Row[] = [];
  selectedYear?: number;
  yearsOfTrips: number[] = [];

  options = {
    title: 'Monthly trip durations',
    hAxis: {title: 'Month'},
    vAxis: {title: 'Total duration (hours)'},
    tooltip: {isHtml: true}
  };

  columnNames = ['Month', 'Total duration'];
  chartName = 'Monthly trip durations';
  type: ChartType = ChartType.ColumnChart;

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
    this.http.get<DurationInMonth[]>(this.baseUrl + 'api/statistics/durations?year=' + this.selectedYear).subscribe(
      (response) => {
        this.durationData = response.map(el => {
          return [el.month, el.totalDuration];
        });
      }
    );
  }

}
