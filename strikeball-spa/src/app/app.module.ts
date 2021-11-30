import { OverlayModule } from '@angular/cdk/overlay';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatToolbarModule } from '@angular/material/toolbar';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthModule } from './auth/auth.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    MatToolbarModule,
    MatGridListModule,
    BrowserModule,
    AuthModule,
    AppRoutingModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,

    MatCardModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatSlideToggleModule,
    OverlayModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
