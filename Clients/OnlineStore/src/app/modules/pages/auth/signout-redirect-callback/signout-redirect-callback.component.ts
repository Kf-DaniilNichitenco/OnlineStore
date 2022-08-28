import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { catchError, takeUntil, tap } from 'rxjs/operators';
import { AuthService } from 'src/app/common/services/auth.service';
import { NgOnDestroy } from 'src/app/common/services/ng-on-destroy.service';
import { RouteConstants } from 'src/app/constants/route.constants';

@Component({
  templateUrl: './signout-redirect-callback.component.html',
  styleUrls: ['./signout-redirect-callback.component.scss'],
  providers: [NgOnDestroy],
})
export class SignoutRedirectCallbackComponent implements OnInit {
  public constructor(
    private readonly destroyed$: NgOnDestroy,
    private _authService: AuthService,
    private _router: Router
  ) {}

  public ngOnInit(): void {
    this._authService
      .finishLogout()
      .pipe(
        takeUntil(this.destroyed$),
        tap(() => console.log('sign out callback')),
        catchError((error) => {
          console.error(error);
          this._router.navigate([`/${RouteConstants.notFound}`]);
          return of();
        })
      )
      .subscribe(() => this._router.navigate(['/'], { replaceUrl: true }));
  }
}
