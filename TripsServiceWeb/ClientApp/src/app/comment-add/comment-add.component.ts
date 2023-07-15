import {Component, Input} from '@angular/core';
import {CommentCreateDTO, CommentCreateValidationErrors} from "../../models/comments";
import {CommentsService} from "../../services/comment.service";

@Component({
  selector: 'app-comment-add-form',
  templateUrl: './comment-add.component.html'
})
export class CommentAddComponent {
  comment: CommentCreateDTO = new CommentCreateDTO();
  validationErrors: CommentCreateValidationErrors = {};

  @Input() tripId: number = 0;

  constructor(private commentService: CommentsService) {
  }

  addComment() {
    this.comment.TripId = this.tripId;
    this.commentService.add(this.comment).subscribe({
      next: () => {
        //Insert new comment data to the parent page
      },
      error: (error) => {
        this.validationErrors = error.error.errors || error.error;
      }
    });
  }
}
