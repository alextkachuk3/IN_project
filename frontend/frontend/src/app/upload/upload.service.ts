import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  constructor(private http: HttpClient) { }

  uploadMusic(file: File, name: string, coverFile: File | null): Observable<any> {
    const formData: FormData = new FormData();
    formData.append('musicFile', file);
    formData.append('musicName', name);
    if (coverFile) {
      formData.append('coverFile', coverFile);
    }    

    const headers = new HttpHeaders({
      Authorization: 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.post(`https://backend20231220034952.azurewebsites.net/Music/Upload`, formData, { headers });
  }
}
