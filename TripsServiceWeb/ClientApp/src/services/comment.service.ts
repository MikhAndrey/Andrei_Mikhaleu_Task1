import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {CommentCreateDTO} from "../models/comments";

@Injectable({ providedIn: 'root' })
export class CommentsService {

  private readonly apiUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/comments";
  }

  add(comment: CommentCreateDTO): Observable<CommentCreateDTO> {
    return this.http.post<CommentCreateDTO>(this.apiUrl + '/add', comment);
  }
}
