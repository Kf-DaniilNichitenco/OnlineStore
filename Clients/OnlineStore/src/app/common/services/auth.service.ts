import { Injectable } from '@angular/core';
import { UserManager, User, UserManagerSettings } from 'oidc-client';
import { Subject } from 'rxjs/internal/Subject';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { RouteConstants } from 'src/app/constants/route.constants';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _userManager: UserManager;
  private _user?: User;
  private _loginChangedSubject = new Subject<boolean>();

  public loginChanged = this._loginChangedSubject.asObservable();

  constructor(private _http: HttpClient) {
    this._userManager = new UserManager(this.idpSettings);
  }

  public login = () => {
    return this._userManager.signinRedirect();
  }

  public signup = () => {
    window.location.href = `${environment.idpAuthority}/account/signup?redirectUrl=${environment.clientRoot}/${RouteConstants.signupRedirectCallbackFullPath}`;
    // return this.http.get(`${environment.idpAuthority}/account/signup?redirectUrl=${environment.clientRoot}/signin-callback`).toPromise()
    //   .then(_ => this.login());
  }

  public getAccessToken = (): Promise<string | null> => {
    return this._userManager.getUser()
      .then(user => {
         return !!user && !user.expired ? user.access_token : null;
    });
  }

  public getRefreshToken = (): Promise<string | null> => {
    return this._userManager.getUser()
      .then(user => {
         return !!user && !!user.refresh_token ? user.refresh_token : null;
    });
  }

  public isAuthenticated = (): Promise<boolean> => {
    return this._userManager.getUser()
    .then(user => {
      if(this._user !== user){
        this._loginChangedSubject.next(user !== null && this.checkUser(user));
      }

      return this.checkUser(this._user);
    })
  }

  public finishLogin = (): Promise<User> => {
    return this._userManager.signinRedirectCallback()
    .then(user => {
      this._user = user;
      console.log(user);
      this._loginChangedSubject.next(this.checkUser(user));
      return user;
    })
  }

  public logout = () => {
    this._userManager.signoutRedirect();
  }

  public finishLogout = () => {
    this._user = undefined;
    return this._userManager.signoutRedirectCallback();
  }

  private checkUser = (user?: User): boolean => {
    return !!user && !user.expired;
  }

  private get idpSettings() : UserManagerSettings {
    return {
      authority: environment.idpAuthority,
      client_id: environment.clientId,
      client_secret: environment.clientSecret,
      redirect_uri: `${environment.clientRoot}/${RouteConstants.signinRedirectCallbackFullPath}`,
      scope: "openid profile full offline_access",
      response_type: "code",
      post_logout_redirect_uri: `${environment.clientRoot}/${RouteConstants.signoutRedirectCallbackFullPath}`
    }
  }
}
