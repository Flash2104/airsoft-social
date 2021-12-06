import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { filter, map, Observable, Subject, takeUntil } from 'rxjs';
import { IProfileData } from './../../shared/services/dto-models/profile/profile-data';
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

  public profile$: Observable<IProfileData | null> =
    this._profileRepo.profile$.pipe(
      filter((p) => p != null),
      map((p) => {
        const sanitized = this._sanitizer.bypassSecurityTrustResourceUrl(
          'data:image/png;base64, ' + p?.avatarData
        );
        return { ...p, avatarData: sanitized } as IProfileData;
      }),
      takeUntil(this._destroy$)
    );

  constructor(
    private _profileService: ProfileService,
    private _profileRepo: ProfileRepository,
    private _sanitizer: DomSanitizer
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
