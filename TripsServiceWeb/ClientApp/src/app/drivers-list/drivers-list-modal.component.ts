import {Component, OnInit} from '@angular/core';
import {DriversService} from "../../services/drivers.service";


@Component({
  selector: 'app-drivers-list-modal',
  template: `
    <div class="modal fade" id="driverListModal" tabindex="-1" aria-labelledby="driverListModalLabel"
         aria-hidden="true">
      <div class="modal-dialog" style="max-width: 60%;">
        <div class="modal-content tp-form">
          <div class="modal-header">
            <h5 class="modal-title tp-label" id="exampleModalLabel">Click on a driver you want to select for a trip</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Select later"></button>
          </div>
          <div class="modal-body" id="drivers-container">

          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" id="select-driver-later-button" data-bs-dismiss="modal">
              Select later
            </button>
            <button type="button" class="btn btn-primary" id="select-driver-button">Apply</button>
          </div>
        </div>
      </div>
    </div>
  `
})
export class DriversListModalComponent implements OnInit {
  constructor(private driverService: DriversService) {
  }

  ngOnInit(): void {
    this.driverService.getAll().subscribe({
      next: (drivers) => {
        console.log(drivers);
      },
      error: (error) => {
        console.log(error);
      }
    });
  }
}
