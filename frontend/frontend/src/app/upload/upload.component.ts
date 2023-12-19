import { Component } from "@angular/core";
import { Router, RouterLink } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { HttpClient } from "@angular/common/http";

import { AuthorizationService } from "../authorization/authorization.service";
import { UploadService } from "./upload.service";

@Component({
  selector: "upload",
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './upload.component.html',
  providers: [UploadService, AuthorizationService, FormsModule]
})
export class UploadComponent {
  musicName: string = '';
  selectedFile: File | null = null;
  constructor(private uploadService: UploadService, private authService: AuthorizationService, private http: HttpClient, private router: Router) { }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0] as File;
  }

  onSubmit(): void {
    if (this.selectedFile) {

      this.uploadService.uploadMusic(this.selectedFile, this.musicName)
        .subscribe(
          response => {
            console.log('File uploaded successfully', response);
            this.router.navigate(['']);
          },
          error => {
            console.error('Error uploading file', error);
          }
        );
    }
  }
}
