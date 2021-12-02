import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-empty-toolbar',
  templateUrl: './empty-toolbar.component.html',
  styleUrls: ['./empty-toolbar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EmptyToolbarComponent implements OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  constructor() {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }
}
