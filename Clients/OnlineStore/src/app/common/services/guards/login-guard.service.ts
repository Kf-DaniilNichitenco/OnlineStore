import { Injectable } from '@angular/core';
import { CanActivate, CanLoad } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../auth.service';

@Injectable({
  providedIn: 'root',
})
export class LoginGuardService implements CanActivate, CanLoad {
  public constructor(private _authService: AuthService) {}

  public canActivate(): Observable<boolean> {
    return this._authService.isAuthenticated();
  }
  public canLoad(): Observable<boolean> {
    return this._authService.isAuthenticated();
  }
}
