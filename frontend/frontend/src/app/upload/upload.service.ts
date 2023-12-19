import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  constructor(private http: HttpClient) { }

  uploadMusic(file: File, name: string): Observable<any> {
    const formData: FormData = new FormData();
    formData.append('file', file);
    formData.append('name', name);

    const headers = new HttpHeaders({
      Authorization: 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.post(`https://localhost:7232/Music/Upload`, formData, { headers });
  }
}
