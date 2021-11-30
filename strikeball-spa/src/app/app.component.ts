import { OverlayContainer } from '@angular/cdk/overlay';
import { Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy {
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
  ngOnDestroy(): void {}
}
