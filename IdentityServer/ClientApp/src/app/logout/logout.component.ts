import { Component, OnInit, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { AuthApiService } from '../services/auth-api.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss']
})
export class LogoutComponent implements OnInit {
  public showGoodbyeMessage: boolean = false;
  public showSignOutIFrame: boolean = false;
  public signOutIFrameUrl: string;

  constructor(private auth: AuthApiService, private route: ActivatedRoute, @Inject(DOCUMENT) private document: Document) { }

  ngOnInit(): void {
    this.signOutUser();
  }

  private signOutUser(): void {
    this.showSignOutIFrame = false;
    var logoutId = this.getLogoutId();

    this.auth.signOutUser(logoutId)
      .subscribe((response: any) => {

        if (response.signOutIFrameUrl) {
          this.showSignoutIFrame(response.signOutIFrameUrl);
        }

        if (response.postLogoutRedirectUri) {
          this.handlePostLogoutRedirect(response);
        } else {
          this.showGoodbyeMessage = true;
        }

      });
  }

  private getLogoutId(): string {
    return this.route.snapshot.queryParams['logoutId'];
  }

  private showSignoutIFrame(signOutIFrameUrl: string): void {
    this.signOutIFrameUrl = signOutIFrameUrl;
    this.showSignOutIFrame = true;
  }

  private handlePostLogoutRedirect(response: any): void {
    if (response.externalAuth) {
      this.document.location.href = `/externallogin/logout?scheme=${response.externalAuth}&postLogoutRedirectUrl=${encodeURIComponent(response.postLogoutRedirectUri)}`;
    } else {
      this.document.location.href = response.postLogoutRedirectUri;
    }
  }

}
