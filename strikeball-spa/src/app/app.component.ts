import {
  animate,
  animateChild,
  group,
  query,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { OverlayContainer } from '@angular/cdk/overlay';
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
  Renderer2,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { Subject, takeUntil, filter, switchMap } from 'rxjs';
import { AuthRepository } from './shared/repository/auth.repository';
import { ProfileRepository } from './shared/repository/profile.repository';
import { AuthService } from './shared/services/auth.service';
import { ProfileService } from './shared/services/profile.service';

export const slideInAnimation = trigger('routeAnimations', [
  transition('PrivatePages <=> PublicPages', [
    style({ position: 'relative' }),
    query(':enter, :leave', [
      style({
        position: 'absolute',
        top: 0,
        left: 0,
        width: '100%',
      }),
    ]),
    query(':enter', [style({ left: '-100%' })]),
    query(':leave', animateChild()),
    group([
      query(':leave', [animate('400ms ease-out', style({ left: '100%' }))]),
      query(':enter', [animate('400ms ease-out', style({ left: '0%' }))]),
    ]),
    query(':enter', animateChild()),
  ]),
]);

@Component({
  selector: 'air-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [slideInAnimation],
})
export class AppComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();
  form: FormGroup = new FormGroup({
    toggleControl: new FormControl(false),
  });

  constructor(
    public authService: AuthService,
    private _authRepo: AuthRepository,
    private _profileService: ProfileService,
    private _profileRepo: ProfileRepository
  ) {}

  ngOnInit(): void {
    this._authRepo.token$
      .pipe(
        filter((t) => t != null),
        switchMap(() => this._profileService.loadCurrentProfile()),
        takeUntil(this._destroy$)
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();

    this._profileRepo.destroy();
  }

  prepareRoute(outlet: RouterOutlet): string {
    return outlet?.activatedRouteData?.['animation'];
  }
}
