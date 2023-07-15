import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-trip-rating',
  templateUrl: './trip-rating.component.html',
})
export class TripRatingComponent implements OnInit{
  private maxRating: number = 5;

  @Input() actualRating: number = 0;

  ngOnInit(): void {
    document.querySelectorAll(".rating-container")
      .forEach(el => this.applyRatingToElement(el as HTMLElement));
  }

  buildRating(rating: number, container: HTMLElement) {
    const intRating: number = Math.floor(rating);
    const fracRating: number = rating - intRating;
    const stars: NodeListOf<HTMLElement> = container.querySelectorAll(".star-filled");

    for (let i = 0; i < stars.length; i++) {
      if (i < intRating) {
        stars[i].style.width = "100%";
      } else if (i === intRating) {
        stars[i].style.width = `${fracRating * 100}%`;
      } else {
        stars[i].style.width = "0%";
      }
    }
  }

  applyRatingToElement(element: HTMLElement) {
    const starContainer: HTMLElement = element.querySelector(".star-container") as HTMLElement;
    this.buildRating(this.actualRating, starContainer);
    const ratingValueContainer: HTMLElement = element.querySelector(".rating-value") as HTMLElement;
    ratingValueContainer.style.backgroundColor = `rgb(${255 - 51 * this.actualRating}, ${51 * this.actualRating}, 0)`;
  }
}
