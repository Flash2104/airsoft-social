import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'air-main-container',
  templateUrl: './main-container.component.html',
  styleUrls: ['./main-container.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MainContainerComponent {
  constructor() {}
}
