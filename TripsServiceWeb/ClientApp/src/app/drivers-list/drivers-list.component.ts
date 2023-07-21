import {Component, OnInit} from '@angular/core';
import {DriversService} from "../../services/drivers.service";
import {DriverInfoDTO} from "../../models/drivers";

@Component({
  selector: 'app-drivers-list',
  templateUrl: './drivers-list.component.html',
  styleUrls: ['drivers-list-modal.component.css']
})
export class DriversListComponent implements OnInit {
  drivers: DriverInfoDTO[] = [];

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
}
