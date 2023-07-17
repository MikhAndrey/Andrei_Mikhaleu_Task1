import {Component, OnInit} from '@angular/core';
import {TripsService} from "../../services/trips/trips.service";
import {TripReadDTOExtended} from "../../models/trips";

@Component({
  selector: 'app-trips-activity',
  templateUrl: './trips-activity.component.html'
})
export class TripsActivityComponent implements OnInit {
  trips: TripReadDTOExtended[] = [];

  constructor(private tripService: TripsService) {
  }

  ngOnInit(): void {
    this.tripService.getActivity().subscribe({
      next: (response) => {
        this.trips = response;
      },
      error: (error) => {
        alert(error.error);
      }
    });
  }

}
