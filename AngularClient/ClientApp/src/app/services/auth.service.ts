import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User } from 'oidc-client';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private manager = new UserManager(this.getClientSettings());
  private user: User | null;

  constructor() {
    this.manager.getUser().then(user => {
      this.user = user;
    });
  }

  private getClientSettings(): UserManagerSettings {
    return {
      authority: 'https://localhost:5001',
      client_id: 'angular',
      redirect_uri: 'https://localhost:5004/auth-callback',
      post_logout_redirect_uri: 'https://localhost:5004/',
      response_type: 'code',
      scope: 'openid profile email weatherapi'
    };
  }

  public isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }

  public login(postLoginRedirectUri: string | null) {
    if (!postLoginRedirectUri) {
      this.manager.signinRedirect();
    } else {
      this.manager.signinRedirect({ state: postLoginRedirectUri });
    }
  }

  public async completeAuthentication(): Promise<string | null> {
    this.user = await this.manager.signinRedirectCallback();
    if (this.user.state) return this.user.state;
    return null;
  }

  public getAuthorizationHeaderValue(): string {
    if (!this.isAuthenticated()) return '';
    return `${this.user.token_type} ${this.user.access_token}`;
  }

  public getUserName(): string {
    return this.user != null ? this.user.profile.name : '';
  }

  public async signout() {
    await this.manager.signoutRedirect();
  }

}
