import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {ChatCreateDTO, ChatDetailsDTO, ChatListDTO} from "../models/chats";

@Injectable({ providedIn: 'root' })
export class ChatsService {

  private readonly apiUrl: string;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string)
  {
    this.apiUrl = baseUrl + "api/chats";
  }

  getAll(): Observable<ChatListDTO[]> {
    return this.http.get<ChatListDTO[]>(this.apiUrl + '/index');
  }

  add(chat: ChatCreateDTO): Observable<ChatListDTO> {
    return this.http.post<ChatListDTO>(this.apiUrl + '/create', chat);
  }

  getById(id: number): Observable<ChatDetailsDTO> {
    return this.http.get<ChatDetailsDTO>(this.apiUrl + `/${id}`);
  }
}
