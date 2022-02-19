import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthRoutingModule } from './auth-routing.module';
import { SigninRedirectCallbackComponent } from './signin-redirect-callback/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from './signout-redirect-callback/signout-redirect-callback.component';
import { SignupRedirectCallbackComponent } from './signup-redirect-callback/signup-redirect-callback.component';


@NgModule({
  declarations: [
    SigninRedirectCallbackComponent,
    SignoutRedirectCallbackComponent,
    SignupRedirectCallbackComponent
  ],
  imports: [
    CommonModule,
    AuthRoutingModule
  ]
})
export class AuthModule { }
