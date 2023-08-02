import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {ChatCreateDTO, ChatDetailsDTO, ChatLeaveDTO, ChatListDTO, ChatSendMessageDTO} from "../models/chats";

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

  delete(id: number): Observable<any> {
    return this.http.delete(this.apiUrl + `/delete/${id}`);
  }

  getById(id: number): Observable<ChatDetailsDTO> {
    return this.http.get<ChatDetailsDTO>(this.apiUrl + `/${id}`);
  }

  addUserToChat(chatId: number): Observable<number> {
    return this.http.post<number>(this.apiUrl + `/join/${chatId}`, chatId);
  }

  leaveChat(chatId: number, participationId: number): Observable<ChatLeaveDTO> {
    const dto: ChatLeaveDTO = {
      chatId: chatId,
      participationId: participationId
    };
    return this.http.put<ChatLeaveDTO>(this.apiUrl + `/leave`, dto);
  }

  sendMessage(message: ChatSendMessageDTO): Observable<any> {
    return this.http.post<any>(this.apiUrl + `/sendMessage`, message);
  }

  getParticipationId(chatId: number): Observable<number> {
    return this.http.get<number>(this.apiUrl + `/participationId/${chatId}`);
  }
}
