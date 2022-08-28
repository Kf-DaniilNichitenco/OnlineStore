import { Component } from '@angular/core';
import { NgOnDestroy } from './common/services/ng-on-destroy.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [NgOnDestroy],
})
export class AppComponent {}
