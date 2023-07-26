import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {DriversService} from "../../services/drivers.service";
import {DriverDetailsDTO} from "../../models/drivers";

@Component({
  selector: 'app-driver-details',
  templateUrl: './driver-details.component.html'
})
export class DriverDetailsComponent implements OnInit {

  driver: DriverDetailsDTO = new DriverDetailsDTO();

  constructor(
    private route: ActivatedRoute,
    private driversService: DriversService
  ) {
  }

  ngOnInit(): void {
    const id = parseInt(this.route.snapshot.paramMap.get('id')!);
    this.driversService.getById(id).subscribe({
      next: (response) => {
        this.driver = response;
      },
      error: (error) => {
        alert(error.error);
      }
    });
  }

}
