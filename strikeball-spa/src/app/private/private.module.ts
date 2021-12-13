import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTableModule } from '@angular/material/table';
import { MatTreeModule } from '@angular/material/tree';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { ProfileEditComponent } from './profile/edit/profile-edit.component';
import { ProfileContainerComponent } from './profile/profile-main.component';
import { SideNavContainerComponent } from './sidenav-container.component';
import { TeamContainerComponent } from './team/team-main.component';

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
            // children: [
            //   // {path: '', redirectTo: '/profile/edit', pathMatch: 'full'},
            //   {path: 'edit', component: ProfileEditComponent},
            // ]
          },
          {
            path: 'profile/edit',
            component: ProfileEditComponent,
          },
          {
            path: 'team',
            component: TeamContainerComponent,
          },
          {
            path: 'events',
            component: TeamContainerComponent,
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
    TeamContainerComponent,
    ProfileEditComponent,
  ],
  imports: [
    MatButtonModule,
    MatSidenavModule,
    MatGridListModule,
    MatCardModule,
    MatListModule,
    MatIconModule,
    CommonModule,
    MatTableModule,
    MatInputModule,
    FormsModule,
    MatDividerModule,
    MatTreeModule,
    ReactiveFormsModule,
    SharedModule,
    RouterModule.forChild(routes),
  ],
})
export class PrivateModule {}
