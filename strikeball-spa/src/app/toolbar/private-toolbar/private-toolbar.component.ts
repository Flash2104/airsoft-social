import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { filter, map, Observable, Subject, takeUntil } from 'rxjs';
import { ProfileRepository } from 'src/app/shared/repository/profile.repository';
import { ProfileService } from 'src/app/shared/services/profile.service';
import { IProfileData } from 'src/app/shared/services/dto-models/profile/profile-data';
import { AuthService } from '../../shared/services/auth.service';

@Component({
  selector: 'air-private-toolbar',
  templateUrl: './private-toolbar.component.html',
  styleUrls: ['./private-toolbar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PrivateToolbarComponent implements OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();
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
    private _sanitizer: DomSanitizer,
    private _profileRepo: ProfileRepository,private _authService: AuthService) {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  onLogout(): void {
    this._authService.signOut().subscribe();
  }
}
