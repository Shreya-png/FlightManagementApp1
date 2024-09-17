import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { BookingResponse } from '../../models/booking-response.model';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-my-bookings',
  templateUrl: './my-bookings.component.html',
  styleUrls: ['./my-bookings.component.css']
})
export class MyBookingsComponent implements OnInit {
  bookings: BookingResponse[] = [];
  errorMessage: string | null = null;
  

  constructor(private userService: UserService, private router: Router, private authService: AuthService) { }
  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.userService.getBookingsForCurrentUser().subscribe({
      next: (data) => {
        console.log('Received bookings data:', data); // Debugging line
        this.bookings = data;
        this.errorMessage = null;
      },
      error: (err) => {
        console.error('Error fetching bookings', err);
        this.errorMessage = 'Failed to load bookings. Please try again later.';
      }
    });
  }
  navigateToMyHome(): void {
    this.router.navigate(['/home']);
  }

  confirmCancelBooking(bookingId: number): void {
    const confirmation = confirm('Are you sure you want to cancel this booking?');
    if (confirmation) {
      this.cancelBooking(bookingId);
    }
  }

  cancelBooking(bookingId: number): void {
    this.userService.deleteBooking(bookingId).subscribe({
      next: () => {
        this.bookings = this.bookings.filter(booking => booking.booking.bookingId !== bookingId);
        alert('Booking cancelled successfully.');
      },
      error: (err) => {
        console.error('Error cancelling booking', err);
        this.errorMessage = 'Failed to cancel booking. Please try again later.';
      }
    });
  }

  confirmCheckIn(bookingId: number): void {
    if (confirm('Are you sure you want to check-in for this booking?')) {
      const updatedCheckin = {
        checkinStatus: 'confirmed',
        checkinDate: new Date().toISOString()
      };
  
      this.userService.updateCheckInStatus(bookingId, updatedCheckin).subscribe({
        next: () => {
          alert('Check-in confirmed successfully!');
  
          // Update the local bookings array to reflect the checked-in status
          this.bookings = this.bookings.map(booking => 
            booking.booking.bookingId === bookingId ? { 
              ...booking, 
              booking: { 
                ...booking.booking, 
                checkinStatus: 'confirmed' 
              } 
            } : booking
          );
        },
        error: (error) => {
          alert('Failed to confirm check-in. Please try again.');
          console.error('Check-in confirmation failed:', error);
        }
      });
    }
  }
  
  logOutAndNavigateHome(): void {
    this.authService.logout(); // Remove the token from local storage
    this.router.navigate(['/home']); // Navigate to the home page
  }

  // fetchBookings(): void {
  //   this.userService.getBookingsForCurrentUser().subscribe({
  //     next: (response: BookingResponse[]) => {
  //       this.bookings = response;
  //     },
  //     error: (error) => {
  //       this.errorMessage = 'Failed to load bookings.';
  //       console.error('Error loading bookings:', error);
  //     }
  //   });
  // }




  
}
