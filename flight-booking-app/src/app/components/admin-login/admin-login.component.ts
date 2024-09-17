import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-login',
  templateUrl: './admin-login.component.html',
  styleUrl: './admin-login.component.css'
})
export class AdminLoginComponent {
  hideNavbarButtons: boolean = true;
  username: string = '';
  password: string = '';
  constructor(private router: Router) {}

  onLogin() {
    // Replace this with your actual login logic
    if (this.username === 'Ollie123' && this.password === 'Olly@123') {
      // Navigate to admin-dashboard on successful login
      this.router.navigate(['/admin-dashboard']);
    } else {
      // Show error message or handle failed login
      alert('Invalid username or password');
    }
  }
}



