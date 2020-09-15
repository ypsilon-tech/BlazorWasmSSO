import { TestBed } from '@angular/core/testing';

import { ForecastApiService } from './forecast-api.service';

describe('ForecastApiService', () => {
  let service: ForecastApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ForecastApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
