import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service'; // Service to handle registration
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  @Output() toggleForm = new EventEmitter<void>();

  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(8)]],
      role: ['Passenger']
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const registrationData = {
        ...this.registerForm.value,
        role: 'Passenger' // Fixed role as passenger
      };
      console.log('Registration Data:', registrationData); // Log data being sent

      this.authService.register(registrationData).subscribe(
        () => {
          alert('Registration was successful!'); // Show success alert
          //this.router.navigate(['/home']); // Redirect to login page
          this.toggleForm.emit();
        },
        (error) => {
          if (error.status === 400) {
            console.error('Validation error:', error.error); // Server should provide more info
            alert('Registration failed. Please check the provided information and try again.');
          } else {
            console.error('Registration error:', error);
          }
        }
      );
    }
  }
}
