export interface Booking {
    bookingId: number; // Assuming this is a GUID or string identifier
    userId: number; // Foreign key linking to User
    flightId: number; // Foreign key linking to Flight
    bookingReference: string;
    status: 'confirmed' | 'cancelled';
    numberOfSeats: number; // Maximum of 4
    bookingDate: string; // ISO date string (YYYY-MM-DD)
    totalPrice?: number;
  }
  