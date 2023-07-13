import {Component, OnInit} from '@angular/core';
import {ChartType, Row} from "angular-google-charts";
import {YearStatisticsCore} from "../../models/statistics";

@Component({
  selector: 'app-duration-statistics',
  templateUrl: './duration-statistics.component.html',
})
export class DurationStatisticsComponent extends YearStatisticsCore implements OnInit {
  durationData: Row[] = [];

  chartOptions = {
    title: 'Monthly trip durations',
    hAxis: {title: 'Month'},
    vAxis: {title: 'Total duration (hours)'},
    tooltip: {isHtml: true}
  };

  chartColumnNames = ['Month', 'Total duration', {type: 'string', role: 'tooltip', 'p': {'html': true}}];
  chartName = 'Monthly trip durations';
  chartType: ChartType = ChartType.ColumnChart;

  onYearChange(): void {
    this.statisticsService.getDurationsByYear(this.selectedYear).subscribe(
      (response) => {
        this.durationData = this.statisticsService.parseDataForCharts(response);
      }
    );
  }

}
