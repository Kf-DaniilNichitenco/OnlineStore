import {
  Component,
  EventEmitter,
  Input, OnInit,
  Output, ViewChild
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { takeUntil, tap } from 'rxjs/operators';
import { Tag } from 'src/app/autogenerated/catalog/models';
import { NgOnDestroy } from '../../services/ng-on-destroy.service';

@Component({
  selector: 'app-input-search',
  templateUrl: './input-search.component.html',
  styleUrls: ['./input-search.component.scss'],
  providers: [NgOnDestroy],
})
export class InputSearchComponent implements OnInit {
  @ViewChild(MatAutocompleteTrigger)
  public matAutocompleteTrigger?: MatAutocompleteTrigger;

  @Input() public dataSource: {
    id: string;
    shortDescription: string;
    shortName: string;
    tags: Tag[];
  }[] = [];
  @Output() public select = new EventEmitter<any | undefined>();
  @Output() public search = new EventEmitter<string | undefined>();
  @Output() public loadItems = new EventEmitter<string | undefined>();

  public inputControl = new FormControl();
  public inputValue: string = '';

  public constructor(private readonly destroyed$: NgOnDestroy) {}

  public ngOnInit(): void {
    this.inputControl.valueChanges
      .pipe(
        takeUntil(this.destroyed$),
        tap((value: string) => {
          this.onChangeInput();
          this.search.emit(value);
        })
      )
      .subscribe();
  }

  public onChangeInput() {
    this.loadItems.emit(this.inputValue);
  }

  public onFocusOut(): void {
    this.matAutocompleteTrigger?.closePanel();
  }

  public onFocus(): void {
    setTimeout(() => {
      console.log('start onFocus');
      this.matAutocompleteTrigger?.openPanel();
      console.log('end onFocus');
    }, 500);
  }
}
