import { Component } from "@angular/core";
import { Router, RouterLink } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { NgIf } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";

import { AuthorizationService } from "../authorization.service";
import { User } from "../user";

@Component({
  selector: "sign-up",
  standalone: true,
  imports: [NgIf, RouterLink, HttpClientModule, FormsModule],
  templateUrl: './sign-up.component.html',
  providers: [AuthorizationService]
})
export class SignUpComponent {
  error: string = "";
  user: User = new User("", "");
  constructor(private authService: AuthorizationService, private router: Router) { }

  submit(user: User) {
    this.authService.postSignUp(user)
      .subscribe({
        next: (data: any) => {
          this.authService.saveToken(data.token);
          this.router.navigate([''])
        },
        error: error => {
          console.log(error.error.error);
          let error_message = error.error.error;
          if (error_message === "username_already_used") {
            this.error = "Username already used";
          }
          else if (error_message === "password_contains_special_chars") {
            this.error = "Password contains special chars";
          }
          else if (error_message === "username_contains_special_chars") {
            this.error = "Username contains special chars";
          }
          else if (error_message === "username_lenght_is_smaller_than_5") {
            this.error = "Username lenght is smaller than 5";
          }
          else if (error_message === "username_length_is_bigger_than_30") {
            this.error = "Username lenght is bigger than 30";
          }
          else {
            this.error = "";
          }
        }
      });
  }
}
