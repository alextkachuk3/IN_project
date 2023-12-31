import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { User } from "./user";
import { Router } from "@angular/router";

@Injectable()
export class AuthorizationService {
  constructor(private http: HttpClient, private router: Router) { }

  postSignIn(user: User) {
    const body = { username: user.username, password: user.password };
    return this.http.post("https://backend20231220034952.azurewebsites.net//User/Login", body);
  }

  postSignUp(user: User) {
    const body = { username: user.username, password: user.password };
    return this.http.post("https://backend20231220034952.azurewebsites.net//User/Signup", body);
  }

  getUsername() {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.getToken()}`,
    });

    return this.http.get("https://backend20231220034952.azurewebsites.net//User/Username", { headers });
  }

  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['signin']);
  }
}
