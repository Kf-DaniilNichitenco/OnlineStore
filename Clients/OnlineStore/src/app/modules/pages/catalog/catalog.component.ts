import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/common/services/auth.service';

@Component({
  selector: 'app-catalog',
  templateUrl: './catalog.component.html',
  styleUrls: ['./catalog.component.scss'],
})
export class CatalogComponent implements OnInit {
  constructor(private _authService: AuthService) {}

  ngOnInit(): void {}

  public login = () => {
    this._authService.login();
  };

  public logout = () => {
    this._authService.logout();
  };

  public signup = () => {
    this._authService.signup();
  };
}
