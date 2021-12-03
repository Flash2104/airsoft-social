import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';

@Component({
  selector: 'air-public-toolbar',
  templateUrl: './public-toolbar.component.html',
  styleUrls: ['./public-toolbar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PublicToolbarComponent implements OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  constructor() {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }
}
