import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { EmptyToolbarComponent } from './empty-toolbar/empty-toolbar.component';

const routes: Routes = [
  {
    path: '',
    component: EmptyToolbarComponent,
  },
];

@NgModule({
  declarations: [EmptyToolbarComponent],
  imports: [
    MatInputModule,
    MatIconModule,
    RouterModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatToolbarModule,
    MatButtonModule,
    MatDialogModule,
    MatTabsModule,
    SharedModule,

    RouterModule.forChild(routes),
  ],
  exports: [],
})
export class PublicToolbarModule {}
