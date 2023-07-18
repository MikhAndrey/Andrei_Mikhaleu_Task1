import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {FeedbackCreateDTO} from "../models/feedbacks";

@Injectable({ providedIn: 'root' })
export class FeedbacksService {

  private readonly apiUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/feedbacks";
  }

  add(feedback: FeedbackCreateDTO): Observable<FeedbackCreateDTO> {
    return this.http.post<FeedbackCreateDTO>(this.apiUrl + '/create', feedback);
  }

  delete(id: number): Observable<number> {
    return this.http.delete<number>(this.apiUrl + `/delete/${id}`);
  }
}
