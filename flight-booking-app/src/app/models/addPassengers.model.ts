import { Passenger } from './passenger.model';

export class AddPassengers {
  bookingId: number;
  passengers: Passenger[];

  constructor(
    bookingId: number = 0,
    passengers: Passenger[] = []
  ) {
    this.bookingId = bookingId;
    this.passengers = passengers;
  }
}
