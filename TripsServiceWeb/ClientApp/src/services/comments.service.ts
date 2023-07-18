import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {CommentCreateDTO, CommentDTO} from "../models/comments";

@Injectable({ providedIn: 'root' })
export class CommentsService {

  private readonly apiUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/comments";
  }

  add(comment: CommentCreateDTO): Observable<CommentDTO> {
    return this.http.post<CommentDTO>(this.apiUrl + '/add', comment);
  }

  delete(id: number): Observable<number> {
    return this.http.delete<number>(this.apiUrl + `/delete/${id}`);
  }
}
