// src/app/models/booking-response.model.ts
import { Flight } from './flight.model';
import { BookingDto } from './bookingDto.model';
import { Passenger } from './passenger.model';


export interface BookingResponse {
  booking: BookingDto;
  flight: Flight;
  passengers: Passenger[];
}
