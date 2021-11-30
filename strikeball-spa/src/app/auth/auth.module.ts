import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatToolbarModule } from '@angular/material/toolbar';
import { AuthContainerComponent } from './auth-container/auth-container.component';
import { SignInComponent } from './auth-container/sign-in/sign-in.component';
import { AuthRoutingModule } from './auth-routing.module';

@NgModule({
  declarations: [AuthContainerComponent, SignInComponent],
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
