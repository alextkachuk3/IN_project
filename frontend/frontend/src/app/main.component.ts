import { Component, OnInit } from '@angular/core';
import { MusicInfoDto } from './music-info.dto';
import { NgIf, NgFor } from "@angular/common";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Music } from './music';

@Component({
  selector: 'app-main',
  standalone: true,
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css'],
  imports: [NgIf, NgFor],
})
export class MainComponent implements OnInit {
  musicList: MusicInfoDto[] = [];
  public musicPlayer = document.getElementById('musicPlayer') as HTMLAudioElement;
  audioSource: string = '';

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    const headers = new HttpHeaders({
      Authorization: 'Bearer ' + localStorage.getItem('token')
    });

    this.http.get<MusicInfoDto[]>(`https://backend20231220034952.azurewebsites.net/Music/Recomendations`, { headers })
      .subscribe((musicList: MusicInfoDto[]) => {
        this.musicList = musicList;
      });
  }

  getMusicImageSrc(id: string) {
    return `https://backend20231220034952.azurewebsites.net/Music/Cover/${id}`;
  }
}
