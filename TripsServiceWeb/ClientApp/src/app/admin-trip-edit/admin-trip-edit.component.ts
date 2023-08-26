import { Component, OnInit } from '@angular/core';
import {AdminTripEditDTO} from "../../models/trips";
import {TripsService} from "../../services/trips/trips.service";
import {RedirectService} from "../../services/redirect.service";
import {RoutesService} from "../../services/routes.service";
import {ActivatedRoute} from "@angular/router";
import {ImagesService} from "../../services/images.service";
import {MapInitService} from "../../services/mapInit.service";
import {DriverIdService} from "../../services/driverId.service";
import {TripEditComponent} from "../trip-edit/trip-edit.component";
import {UsersService} from "../../services/users.Service";
import {UserListDTO} from "../../models/users";
import {Observable} from "rxjs";

@Component({
  selector: 'app-admin-trip-edit',
  templateUrl: './admin-trip-edit.component.html',
})
export class AdminTripEditComponent extends TripEditComponent implements OnInit {
  trip: AdminTripEditDTO = new AdminTripEditDTO();
  users: UserListDTO[];

  tripInitMethod: Observable<AdminTripEditDTO>
  declare tripSubmitMethod: Observable<AdminTripEditDTO>;

  constructor(
    protected tripService: TripsService,
    protected redirectService: RedirectService,
    protected routesService: RoutesService,
    route: ActivatedRoute,
    protected imagesService: ImagesService,
    protected mapInitService: MapInitService,
    protected driverIdService: DriverIdService,
    protected usersService: UsersService) {
    super(tripService, redirectService, routesService, route, imagesService, mapInitService, driverIdService);
    const id: number = parseInt(route.snapshot.paramMap.get('id')!)
    this.tripInitMethod = this.tripService.getTripForAdminEditing(id);
  }

  override ngOnInit(): void {
    super.ngOnInit();
    this.usersService.getAll().subscribe({
      next: value => this.users = value,
      error: err => alert(err.error)
    });
  }

  override setTripSubmitMethod(form: HTMLFormElement){
    const formData: FormData = new FormData(form);
    this.tripSubmitMethod = this.tripService.adminEdit(this.trip, formData);
  }
}
