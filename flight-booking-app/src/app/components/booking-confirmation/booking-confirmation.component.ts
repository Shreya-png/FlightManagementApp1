import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { BookingResponse } from '../../models/booking-response.model';
import { jsPDF } from 'jspdf';
import html2canvas from 'html2canvas';

@Component({
  selector: 'app-booking-confirmation',
  templateUrl: './booking-confirmation.component.html',
  styleUrls: ['./booking-confirmation.component.css'],
})
export class BookingConfirmationComponent implements OnInit {
  bookingId!: number;
  bookingDetails?: BookingResponse;
  errorMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private UserService: UserService,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    // Get bookingId from route parameters
    this.route.paramMap.subscribe(params => {
      this.bookingId = +params.get('bookingId')!;
      this.getBookingDetails();
    });
  }

  getBookingDetails(): void {
    this.UserService.getBookingDetailsById(this.bookingId).subscribe(
      (response: BookingResponse) => {
        this.bookingDetails = response;
      },
      (error) => {
        this.errorMessage = error;
      }
    );
  }

  downloadPDF(): void {
    const ticketElement = document.querySelector('.ticket-container') as HTMLElement;
    if (!this.bookingDetails) {
      this.errorMessage = 'Booking details are not available.';
      return;
    }
  
    html2canvas(ticketElement).then(canvas => {
      const imgData = canvas.toDataURL('image/png');
      const pdf = new jsPDF('p', 'mm', 'a4');
      const imgWidth = 210;
      const pageHeight = 295;
      const imgHeight = canvas.height * imgWidth / canvas.width;
      const heightLeft = imgHeight;
      let position = 0;
  
      pdf.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);
      pdf.save(`e-ticket-${this.bookingDetails!.booking.bookingId}.pdf`);
    });
  }
  
  logOutAndNavigateHome(): void {
    this.authService.logout(); // Remove the token from local storage
    this.router.navigate(['/home']); // Navigate to the home page
  }

  
  myBookings(): void{
    this.router.navigate(['/my-bookings']);
  }
}
