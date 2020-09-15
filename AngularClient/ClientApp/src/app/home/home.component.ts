import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  constructor(private authService: AuthService) { }

  public isLoggedIn(): boolean {
    return this.authService.isAuthenticated();
  }

  public login() {
    this.authService.login(null);
  }

  public async logout() {
    await this.authService.signout();
  }

  public username(): string {
    return this.authService.getUserName();
  }
}
