import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RouteConstants } from './constants/route.constants';

const routes: Routes = [
  { path: `${RouteConstants.auth}`, loadChildren: () => import('./modules/pages/auth/auth.module').then(m => m.AuthModule) },
  { path: `${RouteConstants.accessDenied}`, loadChildren: () => import('./modules/pages/access-denied/access-denied.module').then(m => m.AccessDeniedModule) },
  { path: `${RouteConstants.notFound}`, loadChildren: () => import('./modules/pages/not-found/not-found.module').then(m => m.NotFoundModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
