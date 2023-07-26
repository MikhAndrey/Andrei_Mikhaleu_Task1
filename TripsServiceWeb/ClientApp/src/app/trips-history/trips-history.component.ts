import {Component, OnDestroy, OnInit} from '@angular/core';
import {TripsListBaseComponent} from "../trips-list-base/trips-list-base.component";

@Component({
  selector: 'app-trips-history',
  templateUrl: '../trips-list-base/trips-list-base.component.html'
})
export class TripsHistoryComponent extends TripsListBaseComponent implements OnInit, OnDestroy{

  override ngOnInit() {
    this.pageHeader = "History of your trips";
    this.dataSourceObservable = this.tripService.getHistory();
    super.ngOnInit();
  }
}
