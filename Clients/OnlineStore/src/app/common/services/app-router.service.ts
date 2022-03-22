import { Injectable } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { RouteConstants } from 'src/app/constants/route.constants';

@Injectable({
  providedIn: 'root'
})
export class AppRouterService {

constructor(private _router: Router) { }

public async navigateNotFoundIfError(
  commands: any[], extras?: NavigationExtras | undefined): Promise<boolean> {
  try {
    return await this._router.navigate(commands, extras);
  } catch (error) {
    console.error(error);
    return await this._router.navigate([`/${RouteConstants.notFound}`]);
  }
}

}
