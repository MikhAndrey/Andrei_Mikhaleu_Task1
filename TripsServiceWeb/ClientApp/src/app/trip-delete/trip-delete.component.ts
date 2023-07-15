import {Component, Input} from '@angular/core';
import {TripsService} from "../../services/trips.service";

@Component({
  selector: 'app-trip-delete',
  templateUrl: './trip-delete.component.html',
})
export class TripDeleteComponent {
  @Input() tripId: number = 0;

  constructor(private tripService: TripsService) { }

  deleteTrip(): void {
    this.tripService.delete(this.tripId).subscribe({
      next: () => {
        document.getElementById("cancel-trip-removal-button")!.click();
      },
      error: (error) => {
        alert(error);
      }
    });
  }

}
