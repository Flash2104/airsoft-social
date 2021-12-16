import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { LoadingComponent } from './components/loading/loading.component';
import { AirDateDirective } from './directives/air-date.directive';

@NgModule({
  declarations: [LoadingComponent, AirDateDirective],
  imports: [MatProgressSpinnerModule, MatSnackBarModule, MatButtonModule],
  exports: [LoadingComponent, AirDateDirective],
})
export class SharedModule {}
