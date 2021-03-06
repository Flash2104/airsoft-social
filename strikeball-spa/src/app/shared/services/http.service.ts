import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, mergeMap, Observable, of, take } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthRepository } from '../repository/auth.repository';
import { ISignInRequest } from './dto-models/auth/sign-in/sign-in-request';
import { ISignInResponse } from './dto-models/auth/sign-in/sign-in-response';
import { ISignUpRequest } from './dto-models/auth/sign-up/sign-up-request';
import { ISignUpResponse } from './dto-models/auth/sign-up/sign-up-response';
import { INavigationDataResponse } from './dto-models/navigations/navigation-data';
import { IGetCurrentProfileResponse } from './dto-models/profile/get-current-profile';
import {
  IUpdateProfileRequest,
  IUpdateProfileResponse,
} from './dto-models/profile/update-profile';
import { IGetCityReferencesResponse } from './dto-models/references/cities/cities-dto';
import { IServerResponse } from './dto-models/server-response';
import {
  ICreateTeamRequest,
  ICreateTeamResponse,
} from './dto-models/team/create/create-team-dto';
import { IGetCurrentTeamResponse } from './dto-models/team/get-current/get-current-team-response';

@Injectable({ providedIn: 'root' })
export class HttpService {
  constructor(private _http: HttpClient, private _authRepo: AuthRepository) {}

  /* #region Auth*/

  authSignIn(
    data: ISignInRequest
  ): Observable<IServerResponse<ISignInResponse>> {
    return of('api/auth/sign-in').pipe(
      mergeMap((url) => this.httpPost<ISignInResponse>(url, data))
    );
  }

  authSignUp(
    data: ISignUpRequest
  ): Observable<IServerResponse<ISignUpResponse>> {
    return of('api/auth/sign-up').pipe(
      mergeMap((url) => this.httpPost<ISignUpResponse>(url, data))
    );
  }

  /* #endregion */

  /* #region Profile*/

  profileGetCurrent(): Observable<IServerResponse<IGetCurrentProfileResponse>> {
    return of('api/member/get-current').pipe(
      mergeMap((url) => this.httpGet<IGetCurrentProfileResponse>(url))
    );
  }

  profileUpdate(
    data: IUpdateProfileRequest
  ): Observable<IServerResponse<IUpdateProfileResponse>> {
    return of('api/member/update').pipe(
      mergeMap((url) => this.httpPut<IUpdateProfileResponse>(url, data))
    );
  }

  /* #endregion */

  /* #region References*/

  loadCities(): Observable<IServerResponse<IGetCityReferencesResponse>> {
    return of('api/references/cities').pipe(
      mergeMap((url) => this.httpGet<IGetCityReferencesResponse>(url))
    );
  }

  /* #endregion */

  /* #region Team*/

  teamGetCurrent(): Observable<IServerResponse<IGetCurrentTeamResponse>> {
    return of('api/team/get-current').pipe(
      mergeMap((url) => this.httpGet<IGetCurrentTeamResponse>(url))
    );
  }

  teamCreate(
    data: ICreateTeamRequest
  ): Observable<IServerResponse<ICreateTeamResponse>> {
    return of('api/team/create').pipe(
      mergeMap((url) => this.httpPost<IGetCurrentTeamResponse>(url, data))
    );
  }

  /* #endregion */

  /* #region Navigation*/

  getUserNavigation(): Observable<IServerResponse<INavigationDataResponse>> {
    return of('api/navigation/get').pipe(
      mergeMap((url) => this.httpGet<INavigationDataResponse>(url))
    );
  }

  /* #endregion */

  private getAuthHttpHeaders(): Observable<HttpHeaders> {
    return this._authRepo.token$.pipe(
      take(1),
      map((auth) => {
        const token: string | null | undefined = auth?.token;
        const headersObj: { [key: string]: string | string[] } = {
          'content-type': 'application/json',
        };
        if (token != null) {
          headersObj.Authorization = 'Bearer ' + token;
        }
        return new HttpHeaders(headersObj);
      })
    );
  }

  private httpGet<T>(path: string): Observable<IServerResponse<T>> {
    return this.getAuthHttpHeaders().pipe(
      mergeMap((headers) =>
        this._http.get<IServerResponse<T>>(environment.proxyUrl + path, {
          headers,
        })
      )
    );
  }

  private httpPost<T>(
    path: string,
    body: unknown
  ): Observable<IServerResponse<T>> {
    return this.getAuthHttpHeaders().pipe(
      mergeMap((headers) =>
        this._http.post<IServerResponse<T>>(environment.proxyUrl + path, body, {
          headers,
        })
      )
    );
  }

  private httpPut<T>(
    path: string,
    body: unknown
  ): Observable<IServerResponse<T>> {
    return this.getAuthHttpHeaders().pipe(
      mergeMap((headers) =>
        this._http.put<IServerResponse<T>>(environment.proxyUrl + path, body, {
          headers,
        })
      )
    );
  }

  private httpFileGet<T>(path: string): Observable<IServerResponse<T>> {
    return this.getAuthHttpHeaders().pipe(
      mergeMap((headers) =>
        this._http.get<IServerResponse<T>>(environment.proxyUrl + path, {
          responseType: 'blob' as 'json',
          headers,
        })
      )
      // eslint-disable-next-line no-use-before-define
      // catchError((err) => throwError(() => tryHandleHttpError(err)))
    );
  }

  /**
   * adds query to url
   *
   * @param urlPath query url
   * @param parameters object with key-value
   * @param lang if service is localized, use language
   */
  private addParamsToUrl(
    urlPath: string,
    parameters: {
      [keys: string]: number | string | boolean | string[] | number[] | null;
    } | null = null,
    lang?: string
  ): string {
    let strParams = null;
    if (parameters != null) {
      // generate query string
      strParams = Object.keys(parameters)
        .filter((key) => parameters[key] != null) // filter null parameters
        .map((key) => {
          if (parameters[key] instanceof Array) {
            const paramsValueArray = parameters[key] as (string | number)[];
            let paramArray: string[] | number[];
            if (typeof paramsValueArray[0] === 'string') {
              paramArray = parameters[key] as string[];
              if (paramArray.length > 0) {
                return paramArray
                  .map((item) => key + '=' + encodeURIComponent(`${item}`))
                  .join('&');
              } else {
                return null;
              }
            } else if (typeof paramsValueArray[0] === 'number') {
              paramArray = parameters[key] as number[];
              if (paramArray.length > 0) {
                return paramArray
                  .map((item) => key + '=' + encodeURIComponent(`${item}`))
                  .join('&');
              } else {
                return null;
              }
            }
          } else {
            // eslint-disable-next-line @typescript-eslint/restrict-template-expressions
            return key + '=' + encodeURIComponent(`${parameters[key]}`);
          }
        })
        .filter((param) => param != null) // fix when array return null or undefined
        .join('&');
    }
    let url = `${urlPath}`;
    if (lang != null) {
      url += `?lang=${lang}`;
    }
    if (strParams != null) {
      url = url + (lang == null ? '?' : '&') + strParams;
    }

    return url;
  }
}

// export const tryHandleHttpError = <T>(err: T): string | T => {
//   if (typeof err === 'string') {
//     return err;
//   } else if (err instanceof HttpErrorResponse) {
//     // try to parse a good response with a human friendly error message
//     let errMsg: string | null =
//       (err != null &&
//         err.error != null &&
//         err.error.error != null &&
//         err.error.error.message) ||
//       null;
//     if (errMsg == null) {
//       // no error message received, take message from http response
//       errMsg = `${err.status}. ${err.statusText}`;
//     }
//     return errMsg;
//   }
//   return err;
// };
