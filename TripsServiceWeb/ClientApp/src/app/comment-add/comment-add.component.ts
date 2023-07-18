﻿import {Component, Input} from '@angular/core';
import {CommentCreateDTO, CommentCreateValidationErrors} from "../../models/comments";
import {CommentsService} from "../../services/comment.service";
import {TripCommentService} from "../../services/trips/tripComment.service";

@Component({
  selector: 'app-comment-add-form',
  templateUrl: './comment-add.component.html'
})
export class CommentAddComponent {
  comment: CommentCreateDTO = new CommentCreateDTO();
  validationErrors: CommentCreateValidationErrors = {};

  @Input() tripId: number;

  constructor(private commentService: CommentsService, private tripCommentService: TripCommentService) {
  }

  addComment() {
    this.comment.TripId = this.tripId;
    this.commentService.add(this.comment).subscribe({
      next: (response) => {
        this.tripCommentService.addCommentForTrip(response);
      },
      error: (error) => {
        this.validationErrors = error.error.errors || error.error;
      }
    });
  }
}
