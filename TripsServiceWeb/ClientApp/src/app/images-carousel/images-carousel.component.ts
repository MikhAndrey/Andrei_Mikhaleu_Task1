import {Component, Input} from "@angular/core";
import {ImageDTO} from "../../models/images";

@Component({
  selector: 'app-images-carousel',
  templateUrl: './images-carousel.component.html',
})
export class ImagesCarouselComponent {
 @Input() images: ImageDTO[];
}
