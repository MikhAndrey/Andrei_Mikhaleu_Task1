import {Component, OnDestroy, OnInit} from '@angular/core';
import {TripsListBaseComponent} from "../trips-list-base/trips-list-base.component";

@Component({
  selector: 'app-trips-list',
  templateUrl: '../trips-list-base/trips-list-base.component.html'
})
export class TripsListComponent extends TripsListBaseComponent implements OnInit, OnDestroy{

  override ngOnInit() {
    this.pageHeader = "Your trips";
    this.dataSourceObservable = this.tripService.getAll();
    super.ngOnInit();
  }
}
