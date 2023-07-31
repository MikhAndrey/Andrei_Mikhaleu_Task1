import {Component, OnDestroy, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {DriversService} from "../../services/drivers.service";
import {DriverDetailsDTO} from "../../models/drivers";
import {FeedbacksWebsocketService} from "../../services/feedbacksWebsocket.service";
import {FeedbackReadDTO, FeedbackUpdateDTO} from "../../models/feedbacks";

@Component({
  selector: 'app-driver-details',
  templateUrl: './driver-details.component.html'
})
export class DriverDetailsComponent implements OnInit, OnDestroy {

  driver: DriverDetailsDTO = new DriverDetailsDTO();

  constructor(
    private route: ActivatedRoute,
    private driversService: DriversService,
    private feedbacksWebsocketService: FeedbacksWebsocketService
  ) {
  }

  ngOnInit(): void {
    const id = parseInt(this.route.snapshot.paramMap.get('id')!);
    this.driversService.getById(id).subscribe({
      next: (response) => {
        this.driver = response;
      },
      error: (error) => {
        alert(error.error);
      }
    });
    this.feedbacksWebsocketService.onDelete = this.feedbackDeleteHandler.bind(this);
    this.feedbacksWebsocketService.onCreate = this.feedbackCreateHandler.bind(this);
    this.feedbacksWebsocketService.onUpdate = this.feedbackUpdateHandler.bind(this);
    this.feedbacksWebsocketService.startConnection();
  }

  ngOnDestroy() {
    this.feedbacksWebsocketService.closeConnection();
  }

  private feedbackDeleteHandler(id: number){
    this.driver.feedbacks = this.driver.feedbacks.filter(f => f.id !== id);
  }

  private feedbackCreateHandler(feedback: FeedbackReadDTO){
    this.driver.feedbacks.push(feedback);
  }

  private feedbackUpdateHandler(feedback: FeedbackUpdateDTO) {
    const feedbackIndex = this.driver.feedbacks.findIndex(f => f.id === feedback.id);
    this.driver.feedbacks[feedbackIndex] = feedback;
  }
}
