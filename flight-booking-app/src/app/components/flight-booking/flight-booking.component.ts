import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Flight } from '../../models/flight.model';
import { BookingDto } from '../../models/bookingDto.model';
import { BookingResponse } from '../../models/booking-response.model';
import { AddPassengers } from '../../models/addPassengers.model';
import { Passenger } from '../../models/passenger.model';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-flight-booking',
  templateUrl: './flight-booking.component.html',
  styleUrls: ['./flight-booking.component.css']
})
export class FlightBookingComponent implements OnInit {
  flight: Flight | undefined;
  booking: BookingDto = {
    bookingId: 0,
    flightId: 0,
    userId: '', // This will be populated from the input field
    numberOfSeats: 0,
    totalPrice: 0,
    bookingDate: new Date().toISOString().split('T')[0],
    status: 'confirmed'
  };
  passengers: Passenger[] = []; // Manage passengers here
  bookingResponse: BookingResponse | null = null;
  passengerMessages: string[] = [];

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['flight']) {
        this.flight = JSON.parse(params['flight']);
        if (this.flight) {
          this.booking.flightId = this.flight.id;
          this.updateTotalPrice();
        }
      }
    });
    
    const userId = this.authService.getUserIdFromToken();
    if (userId) {
      this.booking.userId = userId;
    } else {
      console.error('User ID could not be retrieved from the token');
    }
  }


  updateTotalPrice(): void {
    if (this.flight) {
      this.booking.totalPrice = this.flight.fare * this.passengers.length;
    }
  }

  addNewPassenger(): void {
    if (this.passengers.length < 4) {
      this.passengers.push(new Passenger(this.booking.userId));
      this.updateBookingDetails();
      console.log('Passenger added:', this.passengers[this.passengers.length - 1]);
    } else {
      alert('Maximum of 4 passengers allowed.');
    }
  }

  removePassenger(index: number): void {
    if (this.passengers.length > 0) {
      this.passengers.splice(index, 1);
      this.updateBookingDetails();
    }
  }

  addPassengerDetails(index: number): void {
    console.log('Passenger details added:', this.passengers[index]);
    // Update the passenger details in the backend or elsewhere as needed
  }

  updateBookingDetails(): void {
    this.booking.numberOfSeats = this.passengers.length;
    this.updateTotalPrice();
  }

  isBookingValid(): boolean {
    return this.passengers.length > 0 && this.booking.userId.trim() !== '';
  }
  

  submitBooking(): void {
  if (!this.isBookingValid()) {
    alert('Please enter all required passenger details before confirming the booking.');
    console.error('Booking is not valid.');
    return;
  }

  for (const passenger of this.passengers) {
    if (!passenger.name || !passenger.age || !passenger.gender) {
      alert('Please complete all passenger details before proceeding.');
      return;
    }
  }

  console.log('Booking data:', { ...this.booking, passengers: this.passengers });

  // First, create the booking
  this.userService.createBooking({ ...this.booking, passengers: this.passengers }).subscribe({
    next: (response: BookingResponse) => {
      this.bookingResponse = response;
      console.log('Booking ID:', this.bookingResponse?.booking.bookingId);
      console.log('Total Price:', this.bookingResponse?.booking.totalPrice);
      console.log('Status:', this.bookingResponse?.booking.status);

      // Now that the booking has been successfully created and you have the booking ID,
      // you can add the passengers.
      const addPassengersDto = new AddPassengers(this.bookingResponse?.booking.bookingId, this.passengers);

      this.userService.addPassengers(addPassengersDto).subscribe({
        next: () => {
          this.passengers.forEach((passenger) => {
            this.passengerMessages.push(`Passenger ${passenger.name} has been added.`);
          });
          console.log('Passengers added successfully.');

          // Create a check-in entry
          this.createCheckinEntry(this.bookingResponse?.booking.bookingId);

          // Optionally navigate to a confirmation page
          if (this.bookingResponse) {
            this.router.navigate(['/booking-confirmation', this.bookingResponse.booking.bookingId]);
          }
        },
        error: (error) => {
          console.error('Failed to add passengers:', error);
        }
      });
    },
    error: (error) => {
      console.error('Booking failed', error);
      if (error.error && error.error.errors) {
        console.error('Validation errors:', error.error.errors);
      }
    }
  });
}

  createCheckinEntry(bookingId: number | undefined): void {
    if (!bookingId) {
      console.error('Booking ID is not available for check-in.');
      return;
    }

    const checkInData = {
      bookingId: bookingId,
      checkInDate: new Date(),
      checkInStatus: 'pending'
    };

    this.userService.createCheckin(checkInData).subscribe({
      next: (response) => {
        console.log('Check-in created successfully:', response);
      },
      error: (error) => {
        console.error('Failed to create check-in:', error);
      }
    });
  }

  
}


