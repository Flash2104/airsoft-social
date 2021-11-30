import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatToolbarModule } from '@angular/material/toolbar';
import { AuthRoutingModule } from './auth-routing.module';
import { SignInComponent } from './sign-in/sign-in.component';

@NgModule({
  declarations: [SignInComponent],
  imports: [
    MatInputModule,
    MatIconModule,
    ReactiveFormsModule,
    CommonModule,
    AuthRoutingModule,
    MatCardModule,
    MatToolbarModule,
    MatButtonModule,
    MatDialogModule,
  ],
})
export class AuthModule {}
