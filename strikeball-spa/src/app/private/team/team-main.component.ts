import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { filter, map, Observable, Subject, takeUntil } from 'rxjs';
import {
  IMemberViewData,
  ITeamData,
} from 'src/app/shared/services/dto-models/team/team-data';
import { IReferenceData } from '../../shared/services/dto-models/reference-data';
import { TeamRepository } from './repository/team.repository';
import { TeamService } from './repository/team.service';

@Component({
  selector: 'air-team-main',
  templateUrl: './team-main.component.html',
  styleUrls: ['./team-main.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [TeamService, TeamRepository],
})
export class TeamContainerComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  public loading$: Observable<boolean> = this._teamRepo.loading$.pipe(
    takeUntil(this._destroy$)
  );

  public teamLeader$: Observable<IMemberViewData | null | undefined> =
    this._teamRepo.teamLeader$.pipe(takeUntil(this._destroy$));

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
        const roles = m.roles?.sort((a, b) => (a.grade ?? 0) - (b.grade ?? 0));
        return {
          ...m,
          avatar: sanitizedMemberAvatar,
          roles,
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
      .loadCurrentTeam()
      .pipe(takeUntil(this._destroy$))
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  getRolesString(roles: IReferenceData<string>[] | null | undefined): string {
    return roles?.map((x) => x.title)?.join(', ') || '';
  }
}
