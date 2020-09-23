import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { ErrorComponent } from './error/error.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { AuthApiService } from './services/auth-api.service';
import { ErrorsApiService } from './services/errors-api.service';
import { AccountApiService } from './services/account-api.service';
import { LoginProvidersApiService } from './services/login-providers-api.service';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    LogoutComponent,
    ErrorComponent,
    HomeComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    AuthApiService,
    ErrorsApiService,
    AccountApiService,
    LoginProvidersApiService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
