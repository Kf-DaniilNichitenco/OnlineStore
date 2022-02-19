import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/common/services/auth.service';
import { RouteConstants } from 'src/app/constants/route.constants';

@Component({
  templateUrl: './signout-redirect-callback.component.html',
  styleUrls: ['./signout-redirect-callback.component.scss']
})
export class SignoutRedirectCallbackComponent implements OnInit {

  constructor(private _authService: AuthService, private _router: Router) { }

  ngOnInit(): void {
    console.log("sign out callback");
    this._authService.finishLogin()
      .then(_ => this._router.navigate(['/'], {replaceUrl: true}))
      .catch(_ => this._router.navigate([`/${RouteConstants.notFound}`]));
  }

}
