import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { first, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AccountApiService {

  constructor(private http: HttpClient) { }

  public registerUser(name: string, emailAddress: string, username: string, password: string): Observable<any> {
    return this.http.post('api/account', {
          name: name,
          emailAddress: emailAddress,
          username: username,
          password: password
        }, {
          headers: {
            'content-type': 'application/json'
          }
        })
      .pipe(first())
      .pipe(catchError(_ => of({})));
  }
}
