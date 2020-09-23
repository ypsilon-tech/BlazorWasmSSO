import { Component, ViewChild, ElementRef, AfterViewInit, OnInit, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';

import { AuthApiService } from '../services/auth-api.service';
import { LoginProvidersApiService } from '../services/login-providers-api.service';
import { ExternalLoginProvider } from '../models/external-login-provider';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements AfterViewInit, OnInit {

  @ViewChild('inputCtl') inputCtl: ElementRef;

  public username: string;
  public password: string;
  public isError: boolean = false;
  public error: string;
  public isAnyExternalProviders: boolean = false;
  public externalProviders: Array<ExternalLoginProvider>;

  constructor(private auth: AuthApiService, private providers: LoginProvidersApiService, private router: Router, private route: ActivatedRoute, @Inject(DOCUMENT) private document: Document) { }

  public login(): void {
    if (!this.username || !this.password) {
      this.showMissingCredentialsError();
      return;
    }

    this.isError = false;
    let returnUrl = this.getReturnUrl();

    this.auth.loginUser(this.username, this.password, returnUrl)
      .subscribe((result) => {

        if (result.error) {
          this.error = result.error;
          this.isError = true;
          return;
        }

        if (result.redirectTo) {
          this.document.location.href = result.redirectTo;
        }

      });
  }

  public cancel(): void {
    this.isError = false;
    let returnUrl = this.getReturnUrl();

    this.auth.cancelLogin(returnUrl)
      .subscribe((result: any) => {

        if (result.error) {
          this.error = result.error;
          this.isError = true;
          return;
        }

        if (result.redirectTo) {
          this.document.location.href = result.redirectTo;
        }

      });
  }

  public goToRegister(): void {
    let returnUrl = this.getReturnUrl();
    this.router.navigate(['/register'], { queryParams: { ReturnUrl: returnUrl } });
  }

  public externalLogin(provider: ExternalLoginProvider) {
    let returnUrl = this.getReturnUrl();
    this.document.location.href = `/externallogin/authenticate?scheme=${provider.scheme}&returnUrl=${encodeURIComponent(returnUrl)}`;
  }

  private getReturnUrl(): string {
    return this.route.snapshot.queryParams['ReturnUrl'];
  }

  private showMissingCredentialsError(): void {
    this.error = 'Please provide a username and password';
    this.isError = true;
  }

  ngAfterViewInit(): void {
    this.inputCtl.nativeElement.focus();
  }

  ngOnInit(): void {
    let returnUrl = this.getReturnUrl();
    this.providers.getExternalProviders(returnUrl)
      .subscribe(providers => {
        this.externalProviders = providers;
        this.isAnyExternalProviders = (this.externalProviders && this.externalProviders.length > 0);
      });
  }
}
