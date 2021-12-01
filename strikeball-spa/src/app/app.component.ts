import { OverlayContainer } from '@angular/cdk/overlay';
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
  Renderer2,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
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
}
