import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RouteConstants } from 'src/app/constants/route.constants';

import { SigninRedirectCallbackComponent } from './signin-redirect-callback/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from './signout-redirect-callback/signout-redirect-callback.component';
import { SignupRedirectCallbackComponent } from './signup-redirect-callback/signup-redirect-callback.component';

const routes: Routes = [
  { path: `${RouteConstants.signinRedirectCallback}`, component: SigninRedirectCallbackComponent },
  { path: `${RouteConstants.signupRedirectCallback}`, component: SignupRedirectCallbackComponent },
  { path: `${RouteConstants.signoutRedirectCallback}`, component: SignoutRedirectCallbackComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
