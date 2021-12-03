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
import { Subject } from 'rxjs';

export const slideInAnimation = trigger('routeAnimations', [
  transition('PrivatePages <=> PublicPages', [
    style({ position: 'relative' }),
    query(':enter, :leave', [
      style({
        position: 'absolute',
        top: 0,
        left: 0,
        width: '100%',
        transform: 'translate(0, 0)',
      }),
    ]),
    query(':enter', [
      animate('300ms linear', style({ transform: 'translate(0, -100%)' })),
    ]),
    query(':leave', animateChild()),

    group([
      query(':leave', [
        animate('300ms linear', style({ transform: 'translate(0, 100%)' })),
      ]),
      query(':enter', [
        animate('300ms linear', style({ transform: 'translate(0, 0)' })),
      ]),
    ]),
    query(':enter', animateChild()),
  ]),
]);

@Component({
  selector: 'app-root',
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

  constructor(private overlay: OverlayContainer, private renderer: Renderer2) {}
  ngOnInit(): void {
    this.form.controls['toggleControl'].valueChanges.subscribe((lightMode) => {
      const lightClassName = 'light-theme';
      if (lightMode) {
        this.renderer.addClass(document.body, lightClassName);
        this.overlay.getContainerElement().classList.add(lightClassName);
      } else {
        this.renderer.removeClass(document.body, lightClassName);
        this.overlay.getContainerElement().classList.remove(lightClassName);
      }
    });
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  prepareRoute(outlet: RouterOutlet): string {
    return outlet?.activatedRouteData?.['animation'];
  }
}
