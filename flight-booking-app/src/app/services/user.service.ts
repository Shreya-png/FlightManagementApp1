import { Injectable } from '@angular/core';
import { HttpClient , HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Flight } from '../models/flight.model';
import { BookingResponse } from '../models/booking-response.model'; 
import { Booking } from '.././models/booking.model'; // Import the Booking interfaces
import { BookingDto } from '.././models/bookingDto.model';
import { AddPassengers } from '../models/addPassengers.model';
import { AuthService } from './auth.service';


@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = 'http://localhost:5001/api/Bookings';  
  private apiFlightUrl = 'http://localhost:5001/api/Flights';
  private userBookingsUrl = 'http://localhost:5001/api/user-bookings';

  constructor(private http: HttpClient,private authService: AuthService) { }

  //bookings
  getBookings(): Observable<BookingResponse[]> {
    return this.http.get<BookingResponse[]>(`${this.apiUrl}/bookings`);
  }

  bookFlight(booking: BookingDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/bookings`, booking);
  }
  getAvailableFlights(source: string, destination: string, departureDate: string): Observable<Flight[]> {
    return this.http.get<Flight[]>(`${this.apiUrl}/Available_Flights?source=${source}&destination=${destination}&departureDate=${departureDate}`);
  }

  // Add this method if needed to fetch airport data
  getAirports(): Observable<any[]> {
    return this.http.get<any[]>('assets/data/airports.json');
  }

  getCheapestFlight(source: string, destination: string, monthStartDate: string): Observable<Flight> {
    return this.http.get<Flight>(`${this.apiUrl}/CheapestFlight?source=${source}&destination=${destination}&monthStartDate=${monthStartDate}`);
  }

  createBooking(bookingDto: BookingDto): Observable<BookingResponse> {
    return this.http.post<BookingResponse>(this.apiUrl, bookingDto);
  }

  getBookingDetailsById(bookingId: number): Observable<BookingResponse> {
    return this.http.get<BookingResponse>(`${this.apiUrl}/BookingDetails/${bookingId}`);
  }

  getBookingsForCurrentUser(): Observable<BookingResponse[]> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('token')}`);
    return this.http.get<BookingResponse[]>(`${this.apiUrl}/user-bookings`, { headers });
  }

  deleteBooking(bookingId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${bookingId}`);
  }

  //flight functions
  // GET: Displaying List of all Flights
  getFlights(): Observable<Flight[]> {
    return this.http.get<Flight[]>(`${this.apiFlightUrl}/Displaying List of all Flights`);
  }

  // GET: Display Flights by Source & Destination
  getFlightsBySourceDestination(source: string, destination: string): Observable<Flight[]> {
    return this.http.get<Flight[]>(`${this.apiFlightUrl}/Display Flights by Source & Destination?source=${source}&destination=${destination}`);
  }

  // POST: Add a new Flight
  addFlight(flight: Flight): Observable<Flight> {
    return this.http.post<Flight>(this.apiFlightUrl, flight);
  }

  // PUT: Edit Flight by Source, Destination & Flight Number
  updateFlightBySourceDestinationAndNumber(source: string, destination: string, flightNumber: string, updatedFlight: Flight): Observable<void> {
    return this.http.put<void>(`${this.apiFlightUrl}/Edit by Source, Destination & FlightNumber?source=${source}&destination=${destination}&flightNumber=${flightNumber}`, updatedFlight);
  }

  // DELETE: Delete Flight by Flight Number
  deleteFlightByNumber(flightNumber: string): Observable<void> {
    return this.http.delete<void>(`${this.apiFlightUrl}/by Flight Number?flightNumber=${flightNumber}`);
  }

  getBookingsByUserId(userId: string): Observable<BookingResponse[]> {
    return this.http.get<BookingResponse[]>(`${this.apiUrl}/GetBookingsByUserId?userId=${userId}`);
  }
  
  updateBooking(booking: BookingDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/Edit_by_Booking_Id`, booking);
  }

  addPassengers(addPassengers: AddPassengers): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.apiUrl}/AddPassengers`, addPassengers);
}

// user.service.ts
createCheckin(checkInData: { bookingId: number; checkInDate: Date; checkInStatus: string }): Observable<any> {
  return this.http.post('http://localhost:5001/api/checkins', checkInData);
}


updateCheckInStatus(bookingId: number, updatedCheckin: any): Observable<void> {
  return this.http.put<void>(`http://localhost:5001/api/checkins/EditByBookingId/${bookingId}`, updatedCheckin);
}




}
