import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { RouterModule, Routes } from '@angular/router';
import { СommandContainerComponent } from './command/command-container.component';
import { MainContainerComponent } from './main-container.component';
import { ProfileContainerComponent } from './profile/profile-container.component';
const routes: Routes = [
  {
    path: '',
    children: [
      { path: '*', redirectTo: '/', pathMatch: 'full' },
      { path: '', component: MainContainerComponent , children: [
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
      ]},
    ],
  },
];

@NgModule({
  declarations: [
    MainContainerComponent,
    ProfileContainerComponent,
    СommandContainerComponent,
  ],
  imports: [
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    CommonModule,
    RouterModule.forChild(routes),
  ],
})
export class PrivateModule {}
