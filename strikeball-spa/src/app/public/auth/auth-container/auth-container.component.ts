import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable, Subject, takeUntil } from 'rxjs';
import { AuthRepository } from './../repository/auth.repository';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-auth-container',
  templateUrl: './auth-container.component.html',
  styleUrls: ['./auth-container.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [AuthService],
})
export class AuthContainerComponent implements OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  public authLoading$: Observable<boolean> = this._authRepo.loading$.pipe(
    takeUntil(this._destroy$)
  );

  constructor(public dialog: MatDialog, private _authRepo: AuthRepository) {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }
}
