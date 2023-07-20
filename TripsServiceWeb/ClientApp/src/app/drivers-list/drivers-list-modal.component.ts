import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {DriversService} from "../../services/drivers.service";
import {DriverInfoDTO} from "../../models/drivers";

@Component({
  selector: 'app-drivers-list-modal',
  templateUrl: './drivers-list-modal.component.html',
  styleUrls: ['drivers-list-modal.component.css']
})
export class DriversListModalComponent implements OnInit {
  drivers: DriverInfoDTO[] = [];
  selectedDriver: DriverInfoDTO;

  @Output() driverSelected: EventEmitter<DriverInfoDTO> = new EventEmitter();

  constructor(private driverService: DriversService) {
  }

  ngOnInit(): void {
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

  selectDriver(driver: DriverInfoDTO): void {
    this.selectedDriver = driver;
    this.driverSelected.emit(this.selectedDriver);
  }

  resetDriverSelection(){
    this.selectedDriver = new DriverInfoDTO();
    this.driverSelected.emit(this.selectedDriver);
  }
}
