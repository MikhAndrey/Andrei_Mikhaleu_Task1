import {Component, EventEmitter, Output} from '@angular/core';
import {ImageFile} from "../../models/images";

@Component({
  selector: 'app-image-upload',
  templateUrl: './image-upload.component.html',
  styleUrls: ['./image-upload.component.css']
})
export class ImageUploadComponent {
  @Output() filesChanged: EventEmitter<ImageFile[]> = new EventEmitter();

  loadedFiles: ImageFile[] = [];

  handleFileChange(files: FileList | null) {
    if (files != null) {
      Array.from(files).forEach(file => {
        if (file.type.startsWith('image/')) {
          const reader = new FileReader();
          reader.onload = (event: any) => {
            this.loadedFiles.push({file, url: event.target.result});
            this.filesChanged.emit(this.loadedFiles);
          };
          reader.readAsDataURL(file);
        }
      });
    }
  }

  deleteFile(file: ImageFile) {
    const index = this.loadedFiles.indexOf(file);
    if (index >= 0) {
      this.loadedFiles.splice(index, 1);
      this.filesChanged.emit(this.loadedFiles);
    }
  }
}
