import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({ providedIn: 'root' })
export class ImagesService {

  private readonly apiUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/images";
  }

  delete(id: number, tripId: number): Observable<number> {
    return this.http.delete<number>(this.apiUrl + `/delete/${id}/${tripId}`);
  }
}
