import { Component } from "@angular/core";
import { RouterLink } from "@angular/router";
import { NgIf, NgFor } from "@angular/common";

@Component({
  selector: "main",
  imports: [NgIf, NgFor, RouterLink],
  standalone: true,
  templateUrl: "./main.component.html",
  styleUrls: ['./main.component.css'],
})
export class MainComponent {
 
}
