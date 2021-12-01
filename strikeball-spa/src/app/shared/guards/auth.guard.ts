import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { combineLatest, Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import {
  authPersist,
  AuthRepository,
} from '../../public/auth/repository/auth.repository';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private _authRepository: AuthRepository,
    private _router: Router // private _persistQuery: PersistenceQuery, // private _authService: AuthService, // private _deviceInfoQuery: DeviceInfoQuery
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return combineLatest([
      this._authRepository.token$,
      // this._deviceInfoQuery.select((x) => x.deviceId),
      authPersist.initialized$,
    ]).pipe(
      take(1),
      map(([token]) => {
        if (token == null || token.jwt == null) {
          this._router
            .navigate(
              ['public', 'auth']
              // { queryParams: { returnUrl: state.url } }
            )
            .then();
          return false;
        }
        return true;
      })
    );
  }
}
