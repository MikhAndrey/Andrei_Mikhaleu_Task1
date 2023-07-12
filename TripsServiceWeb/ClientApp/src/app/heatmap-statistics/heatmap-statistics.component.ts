import {Component, OnInit} from '@angular/core';
import {HeatmapData} from "@angular/google-maps";
import {YearStatisticsCore} from "../../models/statistics";

@Component({
  selector: 'app-heatmap-statistics',
  templateUrl: './heatmap-statistics.component.html',
})
export class HeatmapStatisticsComponent extends YearStatisticsCore implements OnInit {
  heatmapData: HeatmapData = [];
  heatMapOptions = {
    radius: 40
  };

  onYearChange(): void {
    this.statisticsService.getRoutePointsByYear(this.selectedYear).subscribe(
      (response) => {
        this.heatmapData = response.map(el => {
          return {lat: el.latitude, lng: el.longitude};
        });
      }
    );
  }

}
