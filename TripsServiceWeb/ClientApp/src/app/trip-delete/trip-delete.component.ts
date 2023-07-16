import {Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {TripsService} from "../../services/trips.service";
import {TripIdService} from "../../services/tripId.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-trip-delete',
  templateUrl: './trip-delete.component.html',
})
export class TripDeleteComponent implements OnInit, OnDestroy{
  tripId: number;
  private tripIdSubscription: Subscription;

  @ViewChild('modalCloseButton') modalCloseButton: ElementRef;

  constructor(private tripService: TripsService, private tripIdService: TripIdService) { }

  ngOnInit() {
    this.tripIdSubscription = this.tripIdService.tripId$.subscribe((tripId) => this.tripId = tripId);
  }
  ngOnDestroy(): void {
    this.tripIdSubscription.unsubscribe();
  }

  deleteTrip(): void {
    this.tripService.delete(this.tripId).subscribe({
      next: () => {
        this.modalCloseButton.nativeElement.click();
      },
      error: (error) => {
        alert(error);
      }
    });
  }

}
