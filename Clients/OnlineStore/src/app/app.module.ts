import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ApiModule as CatalogApiModule } from './autogenerated/catalog/api.module';
import { AuthInterceptorService } from './common/services/interceptors/auth-interceptor.service';
import { AngularMaterialModule } from './modules/angular-material/angular-material.module';
import { NavigationBarsModule } from './modules/navigation-bars/navigation-bars.module';
import { AuthModule } from './modules/pages/auth/auth.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    CatalogApiModule.forRoot({ rootUrl: environment.catalogRoot }),
    AuthModule,
    RouterModule,
    AngularMaterialModule,
    NavigationBarsModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
