import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.scss']
})
export class AuthCallbackComponent implements OnInit {
  public error: boolean = false;

  constructor(private router: Router, private authService: AuthService, private route: ActivatedRoute) { }

  async ngOnInit() {
    if (this.route.snapshot.queryParams.hasOwnProperty('error')) {
      this.error = true;
      return;
    }

    let redirectUri = await this.authService.completeAuthentication();
    if (redirectUri !== null) {
      this.router.navigateByUrl(redirectUri);
    } else {
      this.router.navigate(['/home']);
    }
  }

  public login() {
    this.authService.login(null);
  }

}
