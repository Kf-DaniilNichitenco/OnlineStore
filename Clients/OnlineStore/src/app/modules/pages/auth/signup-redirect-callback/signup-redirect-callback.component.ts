import { Component, OnInit } from '@angular/core';
import { takeUntil } from 'rxjs/operators';
import { AuthService } from 'src/app/common/services/auth.service';
import { NgOnDestroy } from 'src/app/common/services/ng-on-destroy.service';

@Component({
  templateUrl: './signup-redirect-callback.component.html',
  styleUrls: ['./signup-redirect-callback.component.scss'],
  providers: [NgOnDestroy],
})
export class SignupRedirectCallbackComponent implements OnInit {
  public constructor(
    private readonly destroyed$: NgOnDestroy,
    private _authService: AuthService
  ) {}

  public ngOnInit(): void {
    this._authService
      .login()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(() => console.log('sign up callback'));
  }
}
