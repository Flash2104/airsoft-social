import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  catchError,
  map,
  mergeMap,
  Observable,
  take,
  tap,
  throwError,
} from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthRepository } from '../../public/auth/repository/auth.repository';
import { IServerResponse } from './dto-models/server-response';

@Injectable({ providedIn: 'root' })
export class HttpService {
  constructor(private _http: HttpClient, private _authRepo: AuthRepository) {}

  /* #region  Beneficiary Account Non-SEPA*/

  // createNonSEPA(
  //   account: IBeneficiaryNonSEPACreateDto
  // ): Observable<IServerResponse<IBeneficiaryNonSEPACreateDto>> {
  //   return of('beneficiaryNonSEPAAccount/create').pipe(
  //     mergeMap((url) =>
  //       this.httpPost<IBeneficiaryNonSEPACreateDto>(url, account)
  //     )
  //   );
  // }

  // getNonSEPAList(
  //   request: IBeneficiaryNonSEPAListRequestDto
  // ): Observable<IServerResponse<IBeneficiaryNonSEPAListResponseDto>> {
  //   return of('beneficiaryNonSEPAAccount/getList').pipe(
  //     map((str) => this.addParamsToUrl(str, { ...request })),
  //     mergeMap((url) => this.httpGet<IBeneficiaryNonSEPAListResponseDto>(url))
  //   );
  // }

  /* #endregion */

  private getAuthHttpHeaders(): Observable<HttpHeaders> {
    return this._authRepo.token$.pipe(
      take(1),
      map((auth) => {
        const token: string | null | undefined = auth?.jwt;
        const headersObj: { [key: string]: string | string[] } = {
          'content-type': 'application/json',
        };
        if (token != null) {
          headersObj.Authorization = token;
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
      ),
      tap((resp) => {
        if (resp.status?.error != null) {
          throw new Error(
            `Code: ${
              resp.status.code != null
                ? resp.status.code
                : resp.status.error.code
            }. Message: ${resp.status.error.message}`
          );
        }
      }),
      catchError((err) => throwError(tryHandleHttpError(err)))
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
      ),
      tap((resp) => {
        if (resp.status?.error != null) {
          throw new Error(
            `Code: ${
              resp.status.code != null
                ? resp.status.code
                : resp.status.error.code
            }. Message: ${resp.status.error.message}`
          );
        }
      }),
      catchError((err) => throwError(tryHandleHttpError(err)))
    );
  }

  private httpFileGet<T>(path: string): Observable<IServerResponse<T>> {
    return this.getAuthHttpHeaders().pipe(
      mergeMap((headers) =>
        this._http.get<IServerResponse<T>>(environment.proxyUrl + path, {
          responseType: 'blob' as 'json',
          headers,
        })
      ),
      tap((resp) => {
        if (resp.status?.error != null) {
          throw new Error(
            `Code: ${
              resp.status.code != null
                ? resp.status.code
                : resp.status.error.code
            }. Message: ${resp.status.error.message}`
          );
        }
      }),
      // eslint-disable-next-line no-use-before-define
      catchError((err) => throwError(tryHandleHttpError(err)))
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

export const tryHandleHttpError = <T>(err: T): string | T => {
  if (typeof err === 'string') {
    return err;
  } else if (err instanceof HttpErrorResponse) {
    // try to parse a good response with a human friendly error message
    let errMsg: string | null =
      (err != null &&
        err.error != null &&
        err.error.error != null &&
        err.error.error.message) ||
      null;
    if (errMsg == null) {
      // no error message received, take message from http response
      errMsg = `${err.status}. ${err.statusText}`;
    }
    return errMsg;
  }
  return err;
};
