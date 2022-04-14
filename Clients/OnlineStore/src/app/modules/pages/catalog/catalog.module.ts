import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CatalogRoutingModule } from './catalog-routing.module';
import { CatalogComponent } from './catalog.component';
import { AngularMaterialModule } from '../../angular-material/angular-material.module';
import { NavigationBarsModule } from '../../navigation-bars/navigation-bars.module';

@NgModule({
  declarations: [CatalogComponent],
  imports: [
    CommonModule,
    CatalogRoutingModule,
    AngularMaterialModule,
    NavigationBarsModule,
  ],
})
export class CatalogModule {}
