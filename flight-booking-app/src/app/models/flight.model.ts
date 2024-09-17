export interface Flight {
  id: number;
  flightNumber: string;
  flightName: string;
  source: string;
  destination: string;
  departureDate: string;  // Change from Date to string
  departureTime: string;
  arrivalDate: string;    // Change from Date to string
  arrivalTime: string;
  fare: number;
}
