import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthModule } from './modules/pages/auth/auth.module';
import { AngularMaterialModule } from './modules/angular-material/angular-material.module';
import { NavigationBarsModule } from './modules/navigation-bars/navigation-bars.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    AuthModule,
    RouterModule,
    AngularMaterialModule,
    NavigationBarsModule,
    NavigationBarsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
