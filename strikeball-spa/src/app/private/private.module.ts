import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { RouterModule, Routes } from '@angular/router';
import { MainContainerComponent } from './main-container.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '*', redirectTo: 'home', pathMatch: 'full' },
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      {
        path: 'home',
        component: MainContainerComponent,
      },
    ],
  },
];

@NgModule({
  declarations: [MainContainerComponent],
  imports: [
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    RouterModule.forChild(routes),
  ],
})
export class PrivateModule {}
