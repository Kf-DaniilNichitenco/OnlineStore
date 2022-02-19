import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/common/services/auth.service';

@Component({
  templateUrl: './signup-redirect-callback.component.html',
  styleUrls: ['./signup-redirect-callback.component.scss']
})
export class SignupRedirectCallbackComponent implements OnInit {

  constructor(private _authService: AuthService) { }

  ngOnInit(): void {
    this._authService.login();
  }

}
