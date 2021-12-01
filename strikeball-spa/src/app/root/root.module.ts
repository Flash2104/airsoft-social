import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../shared/guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '', redirectTo: 'private', pathMatch: 'full' },
      {
        path: 'public',
        loadChildren: () =>
          import('../public/public.module').then((m) => m.PublicModule),
      },
      {
        path: 'private',
        loadChildren: () =>
          import('../private/private.module').then((m) => m.PrivateModule),
        canActivate: [AuthGuard],
      },
    ],
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
})
export class RootModule {}
