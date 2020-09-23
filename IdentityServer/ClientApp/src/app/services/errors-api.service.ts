import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { first } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ErrorsApiService {

  constructor(private http: HttpClient) { }

  public GetErrorDetails(errorId: string): Observable<any> {
    return this.http.get(`api/errors?errorId=${errorId}`)
      .pipe(first());
  }
}
