import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { Observable, Subject, takeUntil } from 'rxjs';
import { ProfileRepository } from './repository/profile.repository';
import { ProfileService } from './repository/profile.service';

@Component({
  selector: 'air-profile-container',
  templateUrl: './profile-container.component.html',
  styleUrls: ['./profile-container.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ProfileRepository, ProfileService],
})
export class ProfileContainerComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  public loading$: Observable<boolean> = this._profileRepo.loading$.pipe(
    takeUntil(this._destroy$)
  );

  constructor(
    private _profileService: ProfileService,
    private _profileRepo: ProfileRepository
  ) {}

  ngOnInit(): void {
    this._profileService.loadCurrentProfile().subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
    this._profileRepo.destroy();
  }
}
