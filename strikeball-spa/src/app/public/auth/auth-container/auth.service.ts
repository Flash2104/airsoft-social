import { Injectable } from '@angular/core';
import { mapTo, Observable, of, tap } from 'rxjs';
import { AuthRepository } from '../repository/auth.repository';
import { ISignInData } from './sign-in/sign-in.component';

@Injectable()
export class AuthService {
  constructor(private _authRepo: AuthRepository) {}

  signIn(data: ISignInData): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._authRepo.setLoading(true);
      }),
      mapTo(void 0)
    );
  }
}
