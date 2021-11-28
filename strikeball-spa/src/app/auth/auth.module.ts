import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SignInComponent } from './sign-in/sign-in.component';
import { AuthRoutingModule } from './auth-routing.module';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';
import {MatIconModule} from '@angular/material/icon';

@NgModule({
  declarations:[
    SignInComponent
  ],
  imports: [
    MatInputModule,
    MatIconModule,
    ReactiveFormsModule,
    CommonModule,
    AuthRoutingModule
  ],
})
export class AuthModule { }
