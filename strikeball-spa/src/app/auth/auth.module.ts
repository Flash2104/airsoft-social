import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { SharedModule } from '../shared-module/shared.module';
import { AuthContainerComponent } from './auth-container/auth-container.component';
import { SignInComponent } from './auth-container/sign-in/sign-in.component';
import { SignUpComponent } from './auth-container/sign-up/sign-up.component';
import { AuthRoutingModule } from './auth-routing.module';

@NgModule({
  declarations: [AuthContainerComponent, SignInComponent, SignUpComponent],
  imports: [
    AuthRoutingModule,
    MatInputModule,
    MatIconModule,
    ReactiveFormsModule,
    CommonModule,
    MatCardModule,
    MatToolbarModule,
    MatButtonModule,
    MatDialogModule,
    MatTabsModule,
    SharedModule,
  ],
})
export class AuthModule {}
