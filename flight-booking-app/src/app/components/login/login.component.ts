import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  loginError: string | null = null;
  jwtHelper = new JwtHelperService();

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.maxLength(15)]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(10)]]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value;

      // Hardcoded admin credentials check
      if (username === 'Ollie123' && password === 'Olly@123') {
        // Navigate to admin-dashboard on successful login
        this.router.navigate(['/admin-dashboard']);
      } else {
        // Proceed with regular login for other users
        this.authService.login({ username, password }).subscribe(
          (response: any) => {
            // Store the token in local storage
            localStorage.setItem('token', response.token);
            console.log(localStorage.getItem('token'));
            console.log(this.authService.getUserIdFromToken());

            const decodedToken = this.jwtHelper.decodeToken(response.token);
            const userRole = decodedToken.role;

            // Redirect based on the user's role
            this.router.navigate(['/flight-search']);

            // Clearing any previous login errors
            this.loginError = null;
          },
          (error) => {
            console.error('Login error:', error);
            // Optionally handle error, e.g., show an error message
            this.loginError = 'Invalid credentials';
          }
        );
      }
    }
  }
}