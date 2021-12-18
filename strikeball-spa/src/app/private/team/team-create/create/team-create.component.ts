import {
  ICitiesData,
  IRegionData,
} from './../../../../shared/services/dto-models/references/cities/cities-dto';
import { CitiesService } from './../../../../shared/services/references/cities.service';
import { Location } from '@angular/common';
import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import {
  debounceTime,
  filter,
  map,
  mapTo,
  Observable,
  ReplaySubject,
  Subject,
  switchMap,
  take,
  takeUntil,
  tap,
  throttleTime,
  withLatestFrom,
} from 'rxjs';
import { CitiesRepository } from 'src/app/shared/repository/cities.repository';
import { TeamCreateRepository } from '../repository/team-create.repository';
import { TeamCreateService } from '../repository/team-create.service';
import { MatSelect } from '@angular/material/select';

@Component({
  selector: 'air-team-create',
  templateUrl: './team-create.component.html',
  styleUrls: ['./team-create.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [CitiesRepository, CitiesService],
})
export class TeamCreateComponent implements OnInit, AfterViewInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  public loading$: Observable<boolean> = this._teamCreateRepo.loading$.pipe(
    takeUntil(this._destroy$)
  );

  form: FormGroup = new FormGroup({
    title: new FormControl(null, Validators.required),
    city: new FormControl(null),
    foundationDate: new FormControl(null),
  });

  public cityFilter: FormControl = new FormControl();
  public filteredCities$: ReplaySubject<IRegionData[]> = new ReplaySubject<
    IRegionData[]
  >(1);

  public loadingCities$: Observable<boolean> = this._citiesRepo.loading$.pipe(
    takeUntil(this._destroy$)
  );

  @ViewChild('citySelect', { static: true }) cityMatSelect:
    | MatSelect
    | undefined;

  constructor(
    private _teamCreateRepo: TeamCreateRepository,
    private _teamCreateService: TeamCreateService,
    private _location: Location,
    private _sanitizer: DomSanitizer,
    private _citiesRepo: CitiesRepository,
    private _citiesService: CitiesService
  ) {}

  // eslint-disable-next-line @angular-eslint/no-empty-lifecycle-method
  ngOnInit(): void {
    this._citiesService.loadCityReferences().subscribe();
    this._citiesRepo.cities$
      .pipe(
        filter((x) => x != null),
        tap((x) => this.filteredCities$.next(x!)),
        takeUntil(this._destroy$)
      )
      .subscribe();
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
        debounceTime(400),
        tap((data) => {
          this._teamCreateRepo.patchCreateData(data);
        }),
        takeUntil(this._destroy$)
      )
      .subscribe();
    this.cityFilter.valueChanges
      .pipe(
        debounceTime(500),
        switchMap((filter) => this.filterCities$(filter)),
        takeUntil(this._destroy$)
      )
      .subscribe();
  }

  ngAfterViewInit(): void {
    this.setInitialValue();
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

  private setInitialValue(): void {
    // this.filteredCities$
    //   .pipe(take(1), takeUntil(this._destroy$))
    //   .subscribe(() => {
    //     // setting the compareWith property to a comparison function
    //     // triggers initializing the selection according to the initial value of
    //     // the form control (i.e. _initializeSelection())
    //     // this needs to be done after the filteredBanks are loaded initially
    //     // and after the mat-option elements are available
    //     if (this.cityMatSelect != null) {
    //       this.cityMatSelect.compareWith = (a: number, b: number) => a === b;
    //     }
    //   });
  }

  private filterCities$(search: string): Observable<void> {
    // filter the banks
    return this._citiesRepo.cities$.pipe(
      take(1),
      map((regions) => {
        if (!search) {
          return regions;
        }
        search = search.toLowerCase();
        const regionsWithFilteredCities = regions?.map((region) => {
          const filtered = region?.cities?.filter(
            (x) => x.cityAddress.toLowerCase().indexOf(search) > -1
          );
          return {
            ...region,
            cities: filtered,
          } as IRegionData;
        });
        return regionsWithFilteredCities?.filter(x => x.cities != null && x.cities.length > 0);
      }),
      tap((x) => this.filteredCities$.next(x!)),
      mapTo(void 0)
    );
  }
}
