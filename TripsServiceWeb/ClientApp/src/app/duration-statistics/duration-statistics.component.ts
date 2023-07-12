import {Component, Inject, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {DurationInMonth} from "../../models/statistics";
import {ChartType, Row} from "angular-google-charts";
import {formatDuration} from "../../utils/formatDuration";

@Component({
  selector: 'app-duration-statistics',
  templateUrl: './duration-statistics.component.html',
})
export class DurationStatisticsComponent implements OnInit {
  durationData: Row[] = [];
  selectedYear?: number;
  yearsOfTrips: number[] = [];

  chartOptions = {
    title: 'Monthly trip durations',
    hAxis: {title: 'Month'},
    vAxis: {title: 'Total duration (hours)'},
    tooltip: {isHtml: true}
  };

  chartColumnNames = ['Month', 'Total duration', { type: 'string', role: 'tooltip', 'p': { 'html': true } }];
  chartName = 'Monthly trip durations';
  chartType: ChartType = ChartType.ColumnChart;

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
          return [el.month, el.totalDuration, `
            <div style="font-weight: bold;">${el.month}:</div>
            </br>
            <div>${formatDuration(el.totalDuration)}</div>
          `];
        });
      }
    );
  }

}
