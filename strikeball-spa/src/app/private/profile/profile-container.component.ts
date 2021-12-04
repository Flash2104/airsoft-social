import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AuthService } from '../../public/auth/auth.service';

@Component({
  selector: 'air-profile-container',
  templateUrl: './profile-container.component.html',
  styleUrls: ['./profile-container.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfileContainerComponent {
  constructor(private _authService: AuthService) {}

  onSubmit(): void {
    this._authService.signOut().subscribe();
  }
}
