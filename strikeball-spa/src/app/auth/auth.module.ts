import { AuthRoutingModule } from './auth-routing.module';
import { SharedModule } from './../shared/shared.module';
import { NgModule } from '@angular/core';
import { AuthContainerComponent } from './auth-container/auth-container.component';
import { SignInComponent } from './auth-container/sign-in/sign-in.component';
import { SignUpComponent } from './auth-container/sign-up/sign-up.component';

@NgModule({
  declarations: [AuthContainerComponent, SignInComponent, SignUpComponent],
  imports: [
    AuthRoutingModule,
    SharedModule
  ],
})
export class AuthModule {}
