import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
// Importing necessary models
import { Flight } from '../../models/flight.model';
import { BookingDto } from '../../models/bookingDto.model';
import { BookingResponse } from '../../models/booking-response.model';
// Importing service class
import { UserService } from '../../services/user.service';

// Extended interface for local use
interface EditableBookingResponse extends BookingResponse {
  isEditing: boolean;
}

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  flight: Flight | undefined;
  booking: BookingDto = {
    bookingId: 0, 
    userId: '', // This should be dynamically set from the auth service or user input
    flightId: 0, // This will be set based on the flight data
    numberOfSeats: 1,
    totalPrice: 0,
    bookingDate: new Date().toISOString().split('T')[0], // Initialize with today's date
    status: 'confirmed' // Default status
  };

  bookingResponse: BookingResponse | null = null;
  userBookings: EditableBookingResponse[] = [];

  constructor(private route: ActivatedRoute, private userService: UserService, private router: Router) {}

  // Fetch bookings by user ID
  getBookingsByUserId(): void {
    const userId = this.booking.userId;

    if (!userId) {
      alert('Please enter a valid User ID.');
      return;
    }

    this.userService.getBookingsByUserId(userId).subscribe({
      next: (bookings: BookingResponse[]) => {
        // Add an `isEditing` property to each booking for tracking edit state
        this.userBookings = bookings.map(booking => ({
          ...booking,
          isEditing: false
        }));
        console.log('User Bookings:', this.userBookings);
      },
      error: (error) => {
        console.error('Failed to fetch bookings', error);
      }
    });
  }

  // Initiate the edit mode for a booking
  editBooking(index: number): void {
    this.userBookings[index].isEditing = true;
  }

  // Confirm cancellation of the booking
  confirmCancel(index: number): void {
    const bookingToUpdate = this.userBookings[index].booking;
    bookingToUpdate.status = 'cancelled'; // Set the status to cancelled

    this.userService.updateBooking(bookingToUpdate).subscribe({
      next: () => {
        alert('Booking cancelled successfully!');
        this.userBookings[index].isEditing = false;
      },
      error: (error) => {
        console.error('Failed to cancel booking:', error);
        if (error.error && error.error.errors) {
          const validationErrors = error.error.errors;
          alert('Validation errors: ' + JSON.stringify(validationErrors));
        } else {
          alert('Failed to cancel booking: ' + error.message);
        }
      }
    });
  }
  
  // Cancel editing and revert changes
  cancelEdit(index: number): void {
    this.userBookings[index].isEditing = false;
    // Optionally, reload the booking data to revert any unsaved changes
    this.getBookingsByUserId();
  }
}
