import {Component, EventEmitter, OnDestroy, OnInit, Output} from '@angular/core';
import {DriversService} from "../../services/drivers.service";
import {DriverInfoDTO} from "../../models/drivers";
import {DriverIdService} from "../../services/driverId.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-drivers-list-modal',
  templateUrl: './drivers-list-modal.component.html',
  styleUrls: ['drivers-list-modal.component.css']
})
export class DriversListModalComponent implements OnInit, OnDestroy {
  drivers: DriverInfoDTO[] = [];
  selectedDriverId?: number;

  driverIdSubscription: Subscription;

  @Output() driverSelected: EventEmitter<DriverInfoDTO> = new EventEmitter();

  constructor(private driverService: DriversService, private driverIdService: DriverIdService) {
  }

  ngOnInit(): void {
    this.driverIdSubscription = this.driverIdService.driverId$.subscribe((id) => {
      this.selectedDriverId = id;
      const driver = this.drivers.find(d => d.id === id);
      this.driverSelected.emit(driver);
    });
    this.driverService.getAll().subscribe({
      next: (drivers) => {
        this.drivers = drivers;
        this.driverService.mapLinks(this.drivers);
      },
      error: (error) => {
        alert("Impossible to load drivers list. Try later");
      }
    });
  }

  ngOnDestroy() {
    this.driverIdSubscription.unsubscribe();
  }

  selectDriver(driver: DriverInfoDTO): void {
    this.selectedDriverId = driver.id;
    this.driverSelected.emit(driver);
  }

  resetDriverSelection(){
    this.driverSelected.emit(new DriverInfoDTO());
  }
}
