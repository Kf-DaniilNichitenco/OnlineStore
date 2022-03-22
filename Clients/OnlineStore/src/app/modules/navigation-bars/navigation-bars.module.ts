import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationBarComponent } from 'src/app/common/components/navigation-bar/navigation-bar.component';
import { AngularMaterialModule } from '../angular-material/angular-material.module';
import { RouterModule } from '@angular/router';
import { HorizontalNavBarComponent } from 'src/app/common/components/horizontal-nav-bar/horizontal-nav-bar.component';
import { SearchesModule } from '../searches/searches.module';



@NgModule({
  declarations: [NavigationBarComponent, HorizontalNavBarComponent],
  imports: [
    CommonModule,
    AngularMaterialModule,
    RouterModule,
    SearchesModule
  ],
  exports: [NavigationBarComponent, HorizontalNavBarComponent]
})
export class NavigationBarsModule { }
