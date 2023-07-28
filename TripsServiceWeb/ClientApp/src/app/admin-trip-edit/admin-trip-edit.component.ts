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

@Component({
  selector: 'app-admin-trip-edit',
  templateUrl: './admin-trip-edit.component.html',
})
export class AdminTripEditComponent extends TripEditComponent implements OnInit {
  trip: AdminTripEditDTO = new AdminTripEditDTO();
  users: UserListDTO[];

  constructor(
    protected tripService: TripsService,
    protected redirectService: RedirectService,
    protected routesService: RoutesService,
    protected route: ActivatedRoute,
    protected imagesService: ImagesService,
    protected mapInitService: MapInitService,
    protected driverIdService: DriverIdService,
    protected usersService: UsersService) {
    super(tripService, redirectService, routesService, route, imagesService, mapInitService, driverIdService);
  }

  override ngOnInit(): void {
    const id: number = parseInt(this.route.snapshot.paramMap.get('id')!);
    this.tripService.getTripForAdminEditing(id).subscribe({
      next: (response) => {
        this.parseIncomingTripData(response);
      },
      error: (error) => alert(error.error)
    });
    this.usersService.getAll().subscribe({
      next: value => this.users = value,
      error: err => alert(err.error)
    });
  }
}
