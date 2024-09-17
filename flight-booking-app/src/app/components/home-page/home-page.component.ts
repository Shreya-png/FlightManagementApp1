import { Component } from '@angular/core';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent {
  showLoginForm = true;
  showRegisterForm = false;

  // Show login form and hide register form
  toggleToLogin() {
    this.showLoginForm = true;
    this.showRegisterForm = false;
  }

  // Show register form and hide login form
  toggleToRegister() {
    this.showLoginForm = false;
    this.showRegisterForm = true;
  }
}
