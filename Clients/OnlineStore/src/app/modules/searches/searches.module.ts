import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputSearchComponent } from 'src/app/common/components/input-search/input-search.component';
import { AngularMaterialModule } from '../angular-material/angular-material.module';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    AngularMaterialModule,
    FormsModule
  ],
  declarations: [InputSearchComponent],
  exports: [InputSearchComponent]
})
export class SearchesModule { }
