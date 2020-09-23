import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { first } from 'rxjs/operators';

import { ExternalLoginProvider } from '../models/external-login-provider';

@Injectable({
  providedIn: 'root'
})
export class LoginProvidersApiService {

  constructor(private http: HttpClient) { }

  public getExternalProviders(returnUrl: string): Observable<Array<ExternalLoginProvider>> {
    return this.http.get<Array<ExternalLoginProvider>>(`api/loginproviders/external?returnUrl=${encodeURIComponent(returnUrl)}`)
      .pipe(first());
  }
}
