import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../shared/guards/auth.guard';
import { EmptyToolbarComponent } from './toolbar/empty-toolbar/empty-toolbar.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '', redirectTo: 'auth', pathMatch: 'full' },
      {
        path: 'auth',
        loadChildren: () =>
          import('./auth/auth.module').then((m) => m.AuthModule),
        canActivate: [AuthGuard],
      },
    ],
  },
];

@NgModule({
  declarations: [EmptyToolbarComponent],
  imports: [RouterModule.forChild(routes)],
})
export class PublicModule {}
