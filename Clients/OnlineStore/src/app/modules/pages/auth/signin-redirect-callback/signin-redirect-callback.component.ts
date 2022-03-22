import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/common/services/auth.service';
import { RouteConstants } from 'src/app/constants/route.constants';

@Component({
  templateUrl: './signin-redirect-callback.component.html',
  styleUrls: ['./signin-redirect-callback.component.scss']
})
export class SigninRedirectCallbackComponent implements OnInit {

  constructor(private _authService: AuthService, private _router: Router) { }

  async ngOnInit(): Promise<void> {
    console.log("sign in callback");

    const user = await this._authService.finishLogin();

    let route =  '/';
    if (user === null) {
      route = RouteConstants.notFound;
    }

    await this._router.navigate([route], {replaceUrl: true});
  }

}
