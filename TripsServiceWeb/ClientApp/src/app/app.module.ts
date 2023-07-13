import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { GoogleMapsModule } from '@angular/google-maps'

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import {RegisterFormComponent} from "./register-form/register-form.component";
import {LoginFormComponent} from "./login-form/login-form.component";
import {UserAccountOptionsComponent} from "./user-account-options/user-account-options.component";
import {HeatmapStatisticsComponent} from "./heatmap-statistics/heatmap-statistics.component";
import {GoogleChartsModule} from "angular-google-charts";
import {DurationStatisticsComponent} from "./duration-statistics/duration-statistics.component";
import {TripCreateComponent} from "./trip-create/trip-create.component";
import {DriversListModalComponent} from "./drivers-list/drivers-list-modal.component";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    RegisterFormComponent,
    LoginFormComponent,
    UserAccountOptionsComponent,
    HeatmapStatisticsComponent,
    DurationStatisticsComponent,
    TripCreateComponent,
    DriversListModalComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    GoogleMapsModule,
    GoogleChartsModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'register', component: RegisterFormComponent },
      { path: 'login', component: LoginFormComponent },
      { path: 'statistics/heatmap', component: HeatmapStatisticsComponent },
      { path: 'statistics/durations', component: DurationStatisticsComponent },
      { path: 'trips/create', component: TripCreateComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
