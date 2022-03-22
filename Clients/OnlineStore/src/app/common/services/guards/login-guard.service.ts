import { Injectable } from '@angular/core';
import { CanActivate, CanLoad, Route, UrlSegment, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../auth.service';

@Injectable({
  providedIn: 'root'
})
export class LoginGuardService implements CanActivate, CanLoad {

  public constructor(private _authService: AuthService) { }

  canLoad(): Promise<boolean> {
    return this.isUserNotAuthenticated();
  }

  public canActivate(): Promise<boolean> {
    return this.isUserNotAuthenticated();
  }

  private async isUserNotAuthenticated(): Promise<boolean> {
    let isAuthenticated: boolean;

    try {
      isAuthenticated = await this._authService.isAuthenticated();
    } catch(error: any) {
      console.error(error);

      isAuthenticated = false;
    }

    return !isAuthenticated;

    // return this._authService.isAuthenticated()
    // .then(res => !res)
    // .catch(error => {
    //   console.error(error);

    //   return true;
    // });
  }
}
