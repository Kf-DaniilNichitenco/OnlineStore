import { Component, OnInit } from '@angular/core';
import { interval, Subscription } from 'rxjs';
import { SearchResult } from '../../models/dtos/search-result';

@Component({
  selector: 'app-horizontal-nav-bar',
  templateUrl: './horizontal-nav-bar.component.html',
  styleUrls: ['./horizontal-nav-bar.component.scss'],
})
export class HorizontalNavBarComponent implements OnInit {

  constructor() { }

  private fakeItems: SearchResult = {
    items: [
      { id: '1', displayedName: 'one'},
      { id: '2', displayedName: 'two'},
      { id: '3', displayedName: 'three'},
      { id: '4', displayedName: 'four'},
      { id: '5', displayedName: 'five'},
    ],

    total: 5
  };

  public items: SearchResult = this.fakeItems;

  public async loadItems(value: string): Promise<void> {
    console.log("start fetch");

    const observable = await new Promise(f => setTimeout(f, 1000));

    let items = this.fakeItems.items;
    if (value.trim() != "") {
      items = items.filter(x => x.displayedName.includes(value));
    }

    this.items.items = items;
    this.items.total = items.length;

    console.log("finish fetch");
  }

  ngOnInit(): void {
  }

}
