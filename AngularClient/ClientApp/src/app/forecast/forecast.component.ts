import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Forecast } from '../models/forecast';
import { ForecastApiService } from '../services/forecast-api.service';

@Component({
  selector: 'app-forecast',
  templateUrl: './forecast.component.html',
  styleUrls: ['./forecast.component.scss']
})
export class ForecastComponent implements OnInit {
  public forecasts: Observable<Array<Forecast>>;
  public isError: boolean = false;
  public errorMessage: string;

  constructor(private forecastService: ForecastApiService) { }

  ngOnInit(): void {
    this.isError = false;
    this.forecasts = this.forecastService.GetForecasts()
      .pipe(catchError(error => this.handleError(error)));
  }

  private handleError(errorResponse: HttpErrorResponse) {
    if (errorResponse.error instanceof ErrorEvent) {
      this.errorMessage = errorResponse.error.message;
    } else {
      this.errorMessage = 'Weather forecast server returned status ' + errorResponse.status;
    }

    this.isError = true;
    return throwError('Error encountered retrieving forecast data');
  }
}
