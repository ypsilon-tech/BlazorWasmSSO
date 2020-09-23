import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { first } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthApiService {

  constructor(private http: HttpClient) { }

  public loginUser(username: string, password: string, returnUrl: string): Observable<any> {
    
    return this.http.post('api/authenticate', {
          requestAction: 'login',
          username: username,
          password: password,
          returnUrl: returnUrl
        }, {
          headers: {
            'content-type': 'application/json'
          }
        })
      .pipe(first());

  }

  public cancelLogin(returnUrl: string): Observable<any> {

    return this.http.post('api/authenticate', {
          requestAction: 'cancel',
          returnUrl: returnUrl
        }, {
          headers: {
            'content-type': 'application/json'
          }
        })
      .pipe(first());
  }

  public signOutUser(logoutId: string): Observable<any> {
    return this.http.get(`api/authenticate/logout?logoutId=${logoutId}`)
      .pipe(first());
  }
}
