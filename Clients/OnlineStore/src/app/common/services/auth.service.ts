import { Injectable } from '@angular/core';
import {
  SignoutResponse,
  User,
  UserManager,
  UserManagerSettings,
} from 'oidc-client';
import { from, Observable, of } from 'rxjs';
import { Subject } from 'rxjs/internal/Subject';
import { catchError, map } from 'rxjs/operators';
import { RouteConstants } from 'src/app/constants/route.constants';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private _userManager: UserManager;
  private _user?: User;
  private _loginChangedSubject = new Subject<boolean>();

  public loginChanged = this._loginChangedSubject.asObservable();

  constructor() {
    this._userManager = new UserManager(this.idpSettings);
  }

  public login(): Observable<void> {
    return from(this._userManager.signinRedirect());
  }

  public signup(): void {
    window.location.href = `${environment.idpAuthority}/account/signup?redirectUrl=${environment.clientRoot}/${RouteConstants.signupRedirectCallbackFullPath}`;
  }

  public getAccessToken(): Observable<string | null> {
    return from(this._userManager.getUser()).pipe(
      map((user) => {
        return user?.access_token ?? null;
      })
    );
  }

  public getRefreshToken(): Observable<string | null> {
    return from(this._userManager.getUser()).pipe(
      map((user) => {
        return user?.refresh_token ?? null;
      })
    );
  }

  public isAuthenticated(): Observable<boolean> {
    return from(this._userManager.getUser()).pipe(
      map((user) => {
        if (this._user !== user) {
          this._loginChangedSubject.next(user !== null && this.checkUser(user));
        }
        return this.checkUser(this._user);
      }),
      catchError(() => of(false))
    );
  }

  public finishLogin(): Observable<User | null> {
    return from(this._userManager.signinRedirectCallback()).pipe(
      map((user) => {
        this._user = user;
        this._loginChangedSubject.next(this.checkUser(user));

        return user;
      }),
      catchError(() => {
        return of(null);
      })
    );
  }

  public logout() {
    this._userManager.signoutRedirect();
  }

  public finishLogout(): Observable<SignoutResponse> {
    return from(this._userManager.signoutRedirectCallback()).pipe(
      map((res) => {
        this._user = undefined;

        return res;
      })
    );
  }

  private checkUser(user?: User): boolean {
    return !!user && !user.expired;
  }

  private get idpSettings(): UserManagerSettings {
    return {
      authority: environment.idpAuthority,
      client_id: environment.clientId,
      client_secret: environment.clientSecret,
      redirect_uri: `${environment.clientRoot}/${RouteConstants.signinRedirectCallbackFullPath}`,
      scope: 'openid profile full offline_access',
      response_type: 'code',
      post_logout_redirect_uri: `${environment.clientRoot}/${RouteConstants.signoutRedirectCallbackFullPath}`,
    };
  }
}
