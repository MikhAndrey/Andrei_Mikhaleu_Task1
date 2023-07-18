import {Injectable} from "@angular/core";
import {Subject} from "rxjs";
import {CommentDTO} from "../../models/comments";

@Injectable({
  providedIn: 'root',
})
export class TripCommentAddService {
  public addedComment$: Subject<CommentDTO> = new Subject<CommentDTO>();

  public addCommentForTrip(comment: CommentDTO) {
    this.addedComment$.next(comment);
  }
}
