import { Component, Output, EventEmitter,Input } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service'; // Adjust the path as needed
@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  @Output() showLogin = new EventEmitter<void>();
  @Output() showRegister = new EventEmitter<void>();
  @Input() hideButtons: boolean = false;
  @Input() showLogoutButton:boolean = false;

  constructor(private router: Router, private authService: AuthService) {}
  // Emit event to show login form
  onLoginClick() {
    this.showLogin.emit();
  }

  // Emit event to show register form
  onRegisterClick() {
    this.showRegister.emit();
  }

  Logout() {
    this.authService.logout(); // Clear the token
    this.router.navigate(['/home']); // Navigate to home page
  }
}
