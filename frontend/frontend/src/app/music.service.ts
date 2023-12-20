import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MusicInfoDto } from './music-info.dto';

@Injectable({
  providedIn: 'root',
})
export class MusicService {
  private apiUrl = 'your_api_url_here';

  constructor(private http: HttpClient) { }

  getMusicInfo(): Observable<MusicInfoDto[]> {
    const headers = new HttpHeaders({
      Authorization: 'Bearer ' + localStorage.getItem('token')
    });
    return this.http.get<MusicInfoDto[]>(`https://localhost:7232/Music/Recomendations`, { headers });
  }

  // Add a method to fetch images
}
