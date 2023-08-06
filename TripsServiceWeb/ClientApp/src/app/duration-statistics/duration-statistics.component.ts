import {Component, OnInit} from '@angular/core';
import {ChartType, Row} from "angular-google-charts";
import {YearStatisticsCore} from "../../models/statistics";
import {StatisticsService} from "../../services/statistics.service";
import {FileStatisticsService} from "../../services/fileStatistics.service";
import {AccountService} from "../../services/account.service";

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

  constructor(
    protected statisticsService: StatisticsService,
    private fileStatisticsService: FileStatisticsService,
    private accountService: AccountService) {
    super(statisticsService);
  }

  onYearChange(): void {
    this.statisticsService.getDurationsByYear(this.selectedYear).subscribe(
      (response) => {
        this.durationData = this.statisticsService.parseDataForCharts(response);
      }
    );
  }

  loadTripsDistanceStatistics(){
    this.fileStatisticsService.exportTripsDistanceDataToPdf().subscribe((data: Blob) => {
      const blob: Blob = new Blob([data], { type: 'application/pdf' });
      const url: string = window.URL.createObjectURL(blob);
      const link: HTMLAnchorElement = document.createElement('a');
      link.href = url;
      link.download = 'Trips_total_distance_stats.pdf';
      link.click();
      window.URL.revokeObjectURL(url);
      link.remove();
    });
  }

  accessToPdfFileDownloadAllowed() {
    return this.accountService.currentUserInfo$.getValue().role === "Admin";
  }
}
