import {AfterViewInit, Component, ElementRef, Input, QueryList, ViewChild, ViewChildren} from '@angular/core';
import {maxRating} from "../appConstants";

@Component({
  selector: 'app-trip-rating',
  templateUrl: './trip-rating.component.html',
})
export class TripRatingComponent implements AfterViewInit{
  starsCount: any[] = new Array(maxRating);

  private _actualRating: number;

  private isViewInited: boolean = false;
  @Input() set actualRating(value: number){
    this._actualRating = value;
    if (this.isViewInited) {
      this.applyRating();
    }
  }

  get actualRating(): number {
    return this._actualRating;
  }

  @ViewChild('starContainer') starContainer: ElementRef;
  @ViewChild('ratingValueContainer') ratingValueContainer: ElementRef;
  @ViewChildren('stars') stars: QueryList<ElementRef>;

  ngAfterViewInit(): void {
    this.applyRating();
    this.isViewInited = true;
  }

  buildRating(rating: number): void {
    const intRating: number = Math.floor(rating);
    const fracRating: number = rating - intRating;

    for (let i = 0; i < this.stars.length; i++) {
      if (i < intRating) {
        this.stars.get(i)!.nativeElement.style.width = "100%";
      } else if (i === intRating) {
        this.stars.get(i)!.nativeElement.style.width = `${fracRating * 100}%`;
      } else {
        this.stars.get(i)!.nativeElement.style.width = "0%";
      }
    }
  }

  applyRating(): void {
    this.buildRating(this._actualRating);
    this.ratingValueContainer.nativeElement.style.backgroundColor = `rgb(${255 - 255 / maxRating * this._actualRating}, ${255 / maxRating * this._actualRating}, 0)`;
  }
}
