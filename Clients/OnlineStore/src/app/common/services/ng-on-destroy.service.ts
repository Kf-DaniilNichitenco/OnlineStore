import { OnDestroy, HostListener, Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class NgOnDestroy extends Subject<null> implements OnDestroy {
  @HostListener('window:beforeunload')
  public ngOnDestroy(): void {
    this.next(null);
    this.complete();
    this.unsubscribe();
  }
}
