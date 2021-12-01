import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-home-container',
  templateUrl: './home-container.component.html',
  styleUrls: ['./home-container.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HomeContainerComponent {
  constructor(public dialog: MatDialog) {}
}
