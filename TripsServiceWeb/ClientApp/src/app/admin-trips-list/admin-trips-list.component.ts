import {Component, OnDestroy, OnInit} from '@angular/core';
import {TripReadDTOExtended} from "../../models/trips";
import {TripsListBaseComponent} from "../trips-list-base/trips-list-base.component";

@Component({
  selector: 'app-admin-trips-list',
  templateUrl: './admin-trips-list.component.html'
})
export class AdminTripsListComponent extends TripsListBaseComponent implements OnInit, OnDestroy{
  trips: TripReadDTOExtended[] = [];

  override ngOnInit() {
    this.pageHeader = "All trips";
    this.dataSourceObservable = this.tripService.getAllUsersTrips();
    super.ngOnInit();
  }
}
