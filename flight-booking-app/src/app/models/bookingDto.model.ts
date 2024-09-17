export interface BookingDto {
    bookingId: number;
    flightId: number;
    userId: string;
    numberOfSeats: number;
    totalPrice: number;
    bookingDate: string; // or Date if you use Date objects
    status: string;
    passengers?: any[]; // Add this line
    
  }
  