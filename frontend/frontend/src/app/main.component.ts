import { Component, OnInit } from '@angular/core';
import { MusicInfoDto } from './music-info.dto';
import { NgIf, NgFor } from "@angular/common";
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-main',
  standalone: true,
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css'],
  imports: [NgIf, NgFor],
})
export class MainComponent implements OnInit {
  musicList: MusicInfoDto[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    const headers = new HttpHeaders({
      Authorization: 'Bearer ' + localStorage.getItem('token')
    });

    this.http.get<MusicInfoDto[]>(`https://localhost:7232/Music/Recomendations`, { headers })
      .subscribe((musicList: MusicInfoDto[]) => {
        this.musicList = musicList;
      });
  }
}
