import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Forecast } from '../models/forecast';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ForecastApiService {

  constructor(private http: HttpClient, private authService: AuthService) { }

  public GetForecasts(): Observable<Array<Forecast>> {
    return this.http.get<Array<Forecast>>('https://localhost:5003/weatherforecast', {
        headers: {
          authorization: this.authService.getAuthorizationHeaderValue()
        }
      });
  }
}
