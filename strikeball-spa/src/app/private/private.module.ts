import { NgModule } from '@angular/core';
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
  imports: [RouterModule.forChild(routes)],
})
export class PrivateModule {}
