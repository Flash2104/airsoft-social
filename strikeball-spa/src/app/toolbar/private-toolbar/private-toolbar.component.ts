import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { AuthService } from '../../shared/services/auth.service';

@Component({
  selector: 'air-private-toolbar',
  templateUrl: './private-toolbar.component.html',
  styleUrls: ['./private-toolbar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PrivateToolbarComponent implements OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  constructor(private _authService: AuthService) {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  onSubmit(): void {
    this._authService.signOut().subscribe();
  }
}
