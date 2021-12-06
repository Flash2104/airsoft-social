import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { СommandContainerComponent } from './command/command-container.component';
import { ProfileContainerComponent } from './profile/profile-container.component';
import { SideNavContainerComponent } from './sidenav-container.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '*', redirectTo: '/', pathMatch: 'full' },
      {
        path: '',
        component: SideNavContainerComponent,
        children: [
          { path: '', redirectTo: 'profile', pathMatch: 'full' },
          {
            path: 'profile',
            component: ProfileContainerComponent,
          },
          {
            path: 'command',
            component: СommandContainerComponent,
          },
          {
            path: 'events',
            component: СommandContainerComponent,
          },
        ],
      },
    ],
  },
];

@NgModule({
  declarations: [
    SideNavContainerComponent,
    ProfileContainerComponent,
    СommandContainerComponent,
  ],
  imports: [
    MatButtonModule,
    MatSidenavModule,
    MatGridListModule,
    MatCardModule,
    MatListModule,
    MatIconModule,
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes),
  ],
})
export class PrivateModule {}
