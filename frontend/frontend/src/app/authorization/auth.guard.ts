import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {
  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (localStorage.getItem('token')) {
      if (route.routeConfig?.path === 'signup' || route.routeConfig?.path === 'signin') {
        this.router.navigate(['']);
        return false;
      }
      else {
        return true;
      }
    } else {
      if (route.routeConfig?.path === 'signup' || route.routeConfig?.path === 'signin') {
        return true;
      }
      else {
        this.router.navigate(['signin']);
        return false;
      }
    }
  }
}
