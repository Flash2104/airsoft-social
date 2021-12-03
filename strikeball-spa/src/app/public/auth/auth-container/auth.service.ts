import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, EMPTY, mapTo, Observable, of, switchMap, tap } from 'rxjs';
import { HttpService } from '../../../shared/services/http.service';
import { SnackbarService } from '../../../shared/services/snackbar.service';
import { AuthRepository } from '../repository/auth.repository';
import { ISignInData } from './sign-in/sign-in.component';
import { ISignUpData } from './sign-up/sign-up.component';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(
    private _http: HttpService,
    private _router: Router,
    private _authRepo: AuthRepository,
    private _snackBarService: SnackbarService
  ) {}

  signIn(data: ISignInData): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._authRepo.setLoading(true);
      }),
      switchMap(() => this._http.authSignIn(data)),
      tap((resp) => {
        if (resp.isSuccess && resp.data?.tokenData?.token != null) {
          this._authRepo.updateUserToken(
            resp.data?.user ?? null,
            resp.data?.tokenData ?? null
          );
          this._router.navigate(['private', 'home']).then((res) => {
            this._authRepo.setLoading(false);
          });
        } else {
          let message = 'Произошла ошибка';
          if (resp.errors != null && resp.errors[0] != null) {
            message = resp.errors[0].message;
          }
          this._snackBarService.showError(message, 'Ошибка');
        }
        this._authRepo.setLoading(false);
      }),
      catchError((err) => {
        this._authRepo.setLoading(false);
        this._snackBarService.showError(err.Message, 'Ошибка');
        return EMPTY;
      }),
      mapTo(void 0)
    );
  }

  signUp(data: ISignUpData): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._authRepo.setLoading(true);
      }),
      switchMap(() => this._http.authSignUp(data)),
      tap((resp) => {
        if (resp.isSuccess && resp.data?.tokenData?.token != null) {
          this._authRepo.updateUserToken(
            resp.data?.user ?? null,
            resp.data?.tokenData ?? null
          );
          this._router.navigate(['private', 'home']).then((res) => {
            this._authRepo.setLoading(false);
          });
        } else {
          let message = 'Произошла ошибка';
          if (resp.errors != null && resp.errors[0] != null) {
            message = resp.errors[0].message;
          }
          this._snackBarService.showError(message, 'Ошибка');
        }
        this._authRepo.setLoading(false);
      }),
      catchError((err) => {
        this._authRepo.setLoading(false);
        this._snackBarService.showError(err.Message, 'Ошибка');
        return EMPTY;
      }),
      mapTo(void 0)
    );
  }

  signOut(): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._authRepo.setLoading(true);
      }),
      tap(() => {
        this._authRepo.updateUserToken(null, null);
        this._router.navigate(['public', 'auth']).then((res) => {
          this._authRepo.setLoading(false);
        });
      }),
      catchError((err) => {
        this._authRepo.setLoading(false);
        return EMPTY;
      }),
      mapTo(void 0)
    );
  }
}
