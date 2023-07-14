import {Component, EventEmitter, Output} from '@angular/core';

@Component({
  selector: 'app-image-upload',
  templateUrl: './image-upload.component.html',
  styleUrls: ['./image-upload.component.css']
})
export class ImageUploadComponent {
  @Output() filesChanged: EventEmitter<{ file: File, url: string }[]> = new EventEmitter();

  loadedFiles: { file: File, url: string }[] = [];

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

  deleteFile(file: { file: File, url: string }) {
    const index = this.loadedFiles.indexOf(file);
    if (index >= 0) {
      this.loadedFiles.splice(index, 1);
      this.filesChanged.emit(this.loadedFiles);
    }
  }
}
