import {HttpClient} from '@angular/common/http';
import {Inject, Injectable} from '@angular/core';
import {Observable} from "rxjs";

@Injectable({ providedIn: 'root' })
export class FileStatisticsService {

  private readonly apiUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/filestats";
  }

  exportTripsDistanceDataToPdf(): Observable<Blob> {
    return this.http.get(this.apiUrl + '/tripsTotalDistance', { responseType: 'blob' });
  }
}
