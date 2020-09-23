import { TestBed } from '@angular/core/testing';

import { LoginProvidersApiService } from './login-providers-api.service';

describe('LoginProvidersApiService', () => {
  let service: LoginProvidersApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LoginProvidersApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
