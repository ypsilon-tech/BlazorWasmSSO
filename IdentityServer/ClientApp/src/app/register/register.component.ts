import { Component, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AccountApiService } from '../services/account-api.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements AfterViewInit {

  @ViewChild('nameCtl') nameCtl: ElementRef;

  public name: string;
  public emailAddress: string;
  public username: string;
  public password: string;
  public isRegisterError: boolean = false;

  constructor(private account: AccountApiService, private router: Router, private route: ActivatedRoute) { }

  public register(): void {
    this.isRegisterError = false;

    this.account.registerUser(this.name, this.emailAddress, this.username, this.password)
      .subscribe((response: any) => {

        if (response.username) {
          let returnUrl = this.getReturnUrl();
          window.location.href = returnUrl;
        } else {
          this.isRegisterError = true;
        }
      });
  }

  public cancel(): void {
    this.goToLoginPage();
  }

  private getReturnUrl(): string {
    return this.route.snapshot.queryParams["ReturnUrl"];
  }

  private goToLoginPage(): void {
    let returnUrl = this.getReturnUrl();
    this.router.navigate(['/login'], { queryParams: { ReturnUrl: returnUrl } });
  }

  ngAfterViewInit(): void {
    this.nameCtl.nativeElement.focus();
  }

}
