import { Component, OnInit, ChangeDetectionStrategy, Input } from '@angular/core';
import { MAT_SELECT_CONFIG } from '@angular/material/select';
import { Observable } from 'rxjs';
import { SearchResult } from '../../models/dtos/search-result';

@Component({
  selector: 'app-input-search',
  templateUrl: './input-search.component.html',
  styleUrls: ['./input-search.component.scss'],
  // providers: [
  //   {
  //     provide: MAT_SELECT_CONFIG,
  //     useValue: { panelClass: 'overlay-search-panel' }
  //   }
  // ],
})
export class InputSearchComponent implements OnInit {

  @Input() public items: SearchResult = {items: [], total: 0};
  @Input() public onSelect: (value?: any) => any = (value?: any) => { console.log('onSelect: ', value);};
  @Input() public onSearch: (value?: any) => any = (value?: any) => { console.log('onSearch: ', value);};
  @Input() public loadItems?: (value: string) => Promise<void>;

  public inputValue: string = "";

  constructor() {
  }

  ngOnInit(): void {
    console.log("items: ", this.items);
  }

  onChangeInput() {
    console.log("this.inputValue: ", this.inputValue);

    console.log(this.loadItems === undefined);

    if (this.loadItems) {
      this.loadItems(this.inputValue);
    }
  }
}
