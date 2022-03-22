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

  constructor() {
    this._userManager = new UserManager(this.idpSettings);
  }

  public login = async (): Promise<void> => {
    return await this._userManager.signinRedirect();
  }

  public signup = () => {
    window.location.href = `${environment.idpAuthority}/account/signup?redirectUrl=${environment.clientRoot}/${RouteConstants.signupRedirectCallbackFullPath}`;
    // return this.http.get(`${environment.idpAuthority}/account/signup?redirectUrl=${environment.clientRoot}/signin-callback`).toPromise()
    //   .then(_ => this.login());
  }

  public getAccessToken = async (): Promise<string | null> => {
    const user = await this._userManager.getUser();
    return !!user && !user.expired ? user.access_token : null;
  }

  public getRefreshToken = async (): Promise<string | null> => {
    const user = await this._userManager.getUser();
    return !!user && !!user.refresh_token ? user.refresh_token : null;
  }

  public isAuthenticated = async (): Promise<boolean> => {
    const user = await this._userManager.getUser();
    if (this._user !== user) {
      this._loginChangedSubject.next(user !== null && this.checkUser(user));
    }

    return this.checkUser(this._user);
  }

  public finishLogin = async (): Promise<User | null> => {
    try {
      const user: User = await this._userManager.signinRedirectCallback();

      this._user = user;
      console.log(user);
      this._loginChangedSubject.next(this.checkUser(user));

      return user;
    } catch (error: any) {
      console.error(error);

      return null;
    }
  }

  public logout = () => {
    this._userManager.signoutRedirect();
  }

  public finishLogout = async () => {
    const res = await this._userManager.signoutRedirectCallback();
    this._user = undefined;
    return res;
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
