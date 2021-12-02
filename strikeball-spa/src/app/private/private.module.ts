import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule, Routes } from '@angular/router';
import { HomeContainerComponent } from './home/home-container.component';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'home',
        component: HomeContainerComponent,
      },
    ],
  },
];

@NgModule({
  declarations: [HomeContainerComponent],
  imports: [MatButtonModule, RouterModule.forChild(routes)],
})
export class PrivateModule {}
