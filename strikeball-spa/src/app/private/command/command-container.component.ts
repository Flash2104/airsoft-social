import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AuthService } from '../../shared/services/auth.service';

@Component({
  selector: 'air-command-container',
  templateUrl: './command-container.component.html',
  styleUrls: ['./command-container.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class СommandContainerComponent {
  constructor(private _authService: AuthService) {}

  onCommandJoin(): void {
    this._authService.signOut().subscribe();
  }

  onCommandCrete(): void {
    this._authService.signOut().subscribe();
  }
}
