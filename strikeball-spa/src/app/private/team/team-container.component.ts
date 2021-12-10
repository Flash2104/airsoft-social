import { TeamService } from './repository/team.service';
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Subject, Observable, takeUntil, filter, map } from 'rxjs';
import { TeamRepository } from './repository/team.repository';
import {
  IMemberViewData,
  ITeamData,
} from 'src/app/shared/services/dto-models/team/team-data';

@Component({
  selector: 'air-team-container',
  templateUrl: './team-container.component.html',
  styleUrls: ['./team-container.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [TeamService, TeamRepository],
})
export class TeamContainerComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  public loading$: Observable<boolean> = this._teamRepo.loading$.pipe(
    takeUntil(this._destroy$)
  );

  public teamLeader$: Observable<IMemberViewData | null | undefined> = this._teamRepo.teamLeader$.pipe(
    takeUntil(this._destroy$)
  );

  public team$: Observable<ITeamData | null> = this._teamRepo.team$.pipe(
    filter((p) => p != null),
    map((team) => {
      const sanitized = this._sanitizer.bypassSecurityTrustResourceUrl(
        'data:image/png;base64, ' + team?.avatar
      );
      const members = team?.members?.map((m) => {
        const sanitizedMemberAvatar =
          this._sanitizer.bypassSecurityTrustResourceUrl(
            'data:image/png;base64, ' + m?.avatar
          );
        return {
          ...m,
          avatar: sanitizedMemberAvatar,
        } as IMemberViewData;
      });
      return { ...team, avatar: sanitized, members } as ITeamData;
    }),
    takeUntil(this._destroy$)
  );

  constructor(
    private _teamRepo: TeamRepository,
    private _teamService: TeamService,
    private _sanitizer: DomSanitizer
  ) {}

  // eslint-disable-next-line @angular-eslint/no-empty-lifecycle-method
  ngOnInit(): void {
    this._teamService
      .loadCurrentProfile()
      .pipe(takeUntil(this._destroy$))
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }
}
