import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { AuthorizationService } from ".././authorization/authorization.service";
import { RouterLink } from "@angular/router";
import { NgIf, NgFor } from "@angular/common";
import { Music } from ".././music";

@Component({
  selector: "my-uploads",
  imports: [NgIf, NgFor, RouterLink],
  standalone: true,
  templateUrl: "./my-uploads.component.html",
  styleUrls: ['./my-uploads.component.css'],
})
export class MyUploadsComponent implements OnInit {
 
  public musics: Music[] = [];
  public musicPlayer = document.getElementById('musicPlayer') as HTMLAudioElement;
  audioSource: string = '';
  constructor(private http: HttpClient, private authService: AuthorizationService) { }

  ngOnInit() {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`,
    });
    
    this.http.get<Music[]>('https://backend20231220034952.azurewebsites.net/Music/My', { headers }).subscribe(
      (data) => {
        data.forEach((element:any) => this.musics.push(element));
        console.log(this.musics);
      },
      (error) => {
        console.error('Error:', error);
      }
    );
  }

  playMusic(musicId: string) {
    this.http.get(`https://backend20231220034952.azurewebsites.net/Music/${musicId}`, { responseType: 'arraybuffer' })
      .subscribe(
        (data: any) => {
          const blob = new Blob([data], { type: 'audio/mpeg' });
          const url = URL.createObjectURL(blob);
          this.audioSource = url;
        },
        (error) => {
          console.error('Error fetching audio content:', error);
        }
      );
  }
}
