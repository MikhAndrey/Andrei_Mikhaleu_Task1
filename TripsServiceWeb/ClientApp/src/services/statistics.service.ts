import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {DurationInMonth, RoutePointsDTO} from "../models/statistics";
import {Row} from "angular-google-charts";
import {formatDuration} from "../utils/formatDuration";

@Injectable({ providedIn: 'root' })
export class StatisticsService {

  private apiUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/statistics";
  }

  getYears(): Observable<number[]> {
    return this.http.get<number[]>(this.apiUrl + '/years');
  }

  getDurationsByYear(year: number): Observable<DurationInMonth[]>{
    return this.http.get<DurationInMonth[]>(this.apiUrl + '/durations?year=' + year);
  }

  getRoutePointsByYear(year: number): Observable<RoutePointsDTO[]>{
    return this.http.get<RoutePointsDTO[]>(this.apiUrl + '/heatmap?year=' + year);
  }

  parseDataForCharts(data: DurationInMonth[]): Row[]{
    return data.map(el => {
      return [el.month, el.totalDuration, `
            <div style="font-weight: bold;">${el.month}:</div>
            </br>
            <div>${formatDuration(el.totalDuration)}</div>
          `];
    });
  }
}
