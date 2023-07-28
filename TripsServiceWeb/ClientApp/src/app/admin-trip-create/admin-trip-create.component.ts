import { Component, OnInit } from '@angular/core';
import {AdminTripCreateDTO} from "../../models/trips";
import {TripsService} from "../../services/trips/trips.service";
import {RedirectService} from "../../services/redirect.service";
import {UsersService} from "../../services/users.Service";
import {UserListDTO} from "../../models/users";
import {TripCreateComponent} from "../trip-create/trip-create.component";

@Component({
  selector: 'app-admin-trip-create',
  templateUrl: './admin-trip-create.component.html'
})
export class AdminTripCreateComponent extends TripCreateComponent implements OnInit {
  trip: AdminTripCreateDTO = new AdminTripCreateDTO();
  users: UserListDTO[];

  constructor(
    protected tripService: TripsService,
    protected redirectService: RedirectService,
    private usersService: UsersService) {
    super(tripService, redirectService);
  }

  override ngOnInit(): void {
    this.usersService.getAll().subscribe({
      next: value => this.users = value,
      error: err => alert(err.error)
    });
  }
}
