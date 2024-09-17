import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css'
})
export class AdminDashboardComponent {
  hideNavbarButtons: boolean = true;

  constructor(private router: Router) {}

  viewFlights() {
    this.router.navigate(['/admin']);
  }

  searchFlights() {
    this.router.navigate(['/search-flights']);
  }
}
