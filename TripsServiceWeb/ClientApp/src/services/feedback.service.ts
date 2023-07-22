import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {FeedbackCreateDTO, FeedbackReadDTO} from "../models/feedbacks";

@Injectable({ providedIn: 'root' })
export class FeedbacksService {

  private readonly apiUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/feedbacks";
  }

  add(feedback: FeedbackCreateDTO): Observable<FeedbackReadDTO> {
    return this.http.post<FeedbackReadDTO>(this.apiUrl + '/create', feedback);
  }

  update(feedback: FeedbackReadDTO): Observable<FeedbackReadDTO> {
    return this.http.put<FeedbackReadDTO>(this.apiUrl + '/update', feedback);
  }

  delete(id: number): Observable<number> {
    return this.http.delete<number>(this.apiUrl + `/delete/${id}`);
  }
}
