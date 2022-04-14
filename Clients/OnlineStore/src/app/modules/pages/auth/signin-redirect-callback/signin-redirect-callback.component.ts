import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { AuthService } from 'src/app/common/services/auth.service';
import { NgOnDestroy } from 'src/app/common/services/ng-on-destroy.service';
import { RouteConstants } from 'src/app/constants/route.constants';

@Component({
  templateUrl: './signin-redirect-callback.component.html',
  styleUrls: ['./signin-redirect-callback.component.scss'],
  providers: [NgOnDestroy],
})
export class SigninRedirectCallbackComponent implements OnInit {
  constructor(
    private readonly destroyed$: NgOnDestroy,
    private _authService: AuthService,
    private _router: Router
  ) {}

  async ngOnInit(): Promise<void> {
    console.log('sign in callback');

    this._authService
      .finishLogin()
      .pipe(takeUntil(this.destroyed$))
      .subscribe((user) => {
        let route = '/';
        if (user === null) {
          route = RouteConstants.notFound;
        }

        this._router.navigate([route], { replaceUrl: true });
      });
  }
}
