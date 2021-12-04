import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AuthService } from '../../shared/services/auth.service';

@Component({
  selector: 'air-home-container',
  templateUrl: './home-container.component.html',
  styleUrls: ['./home-container.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HomeContainerComponent {
  constructor(private _authService: AuthService) {}

  onSubmit(): void {
    this._authService.signOut().subscribe();
  }
}
