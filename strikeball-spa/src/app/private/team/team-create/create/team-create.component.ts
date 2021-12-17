import { Location } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { Observable, Subject, takeUntil, tap, throttleTime } from 'rxjs';
import { TeamCreateRepository } from '../repository/team-create.repository';
import { TeamCreateService } from '../repository/team-create.service';

@Component({
  selector: 'air-team-create',
  templateUrl: './team-create.component.html',
  styleUrls: ['./team-create.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TeamCreateComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  public loading$: Observable<boolean> = this._teamCreateRepo.loading$.pipe(
    takeUntil(this._destroy$)
  );

  form: FormGroup = new FormGroup({
    title: new FormControl(null, Validators.required),
    city: new FormControl(null),
    foundationDate: new FormControl(null),
  });

  constructor(
    private _teamCreateRepo: TeamCreateRepository,
    private _teamCreateService: TeamCreateService,
    private _location: Location,
    private _sanitizer: DomSanitizer
  ) {}

  // eslint-disable-next-line @angular-eslint/no-empty-lifecycle-method
  ngOnInit(): void {
    this._teamCreateRepo.createData$
      .pipe(
        tap((data) => {
          if (data == null) {
            this.form.reset();
          } else {
            this.form.setValue(data, { emitEvent: false });
          }
        }),
        takeUntil(this._destroy$)
      )
      .subscribe();
    // this.form.controls.foundationDate.disable();
    this.form.valueChanges
      .pipe(
        throttleTime(400),
        tap((data) => {
          this._teamCreateRepo.patchCreateData(data);
        }),
        takeUntil(this._destroy$)
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  onCancel(): void {
    this._location.back();
  }

  onSave(): void {
    this._teamCreateService.createTeam().subscribe();
  }
}