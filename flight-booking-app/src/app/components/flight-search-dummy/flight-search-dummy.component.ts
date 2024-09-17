import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { Flight } from '../../models/flight.model';
@Component({
  selector: 'app-flight-search-dummy',
  templateUrl: './flight-search-dummy.component.html',
  styleUrl: './flight-search-dummy.component.css'
})
export class FlightSearchDummyComponent implements OnInit {
  source: string = '';
  destination: string = '';
  departureDate: string = '';
  flights: Flight[] = [];
  sourceSearch: string = '';
  destinationSearch: string = '';
  filteredAirportsSource: any[] = [];
  filteredAirportsDestination: any[] = [];
  private airports: any[] = [];
  minDate: string = '';
  errorMessage: string = '';

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    this.loadAirports();
    this.setMinDate();
  }

  loadAirports(): void {
    this.userService.getAirports().subscribe(
      data => {
        this.airports = data;
        this.filteredAirportsSource = this.airports;
        this.filteredAirportsDestination = this.airports;
      },
      error => {
        console.error('Error fetching airports', error);
      }
    );
  }

  setMinDate(): void {
    const today = new Date();
    const year = today.getFullYear();
    const month = (today.getMonth() + 1).toString().padStart(2, '0');
    const day = today.getDate().toString().padStart(2, '0');
    this.minDate = `${year}-${month}-${day}`;
  }


  // filterAirports(type: 'source' | 'destination'): void {
  //   if (type === 'source') {
  //     this.filteredAirportsSource = this.airports.filter(airport =>
  //       airport.name.toLowerCase().includes(this.sourceSearch.toLowerCase())
  //     );
  //   } else {
  //     this.filteredAirportsDestination = this.airports.filter(airport =>
  //       airport.name.toLowerCase().includes(this.destinationSearch.toLowerCase())
  //     );
  //   }
  // }

  
 

  searchFlights(): void {
    const today=new Date().toISOString().split('T')[0]; 
    if (this.source === this.destination) {
      this.errorMessage = 'Source and destination cannot be the same.';
      return;
    }

    if (this.departureDate < today) {
      this.errorMessage = 'Date cannot be in the past.';
      return;
    }

    

    if (this.source && this.destination && this.departureDate) {
      this.userService.getAvailableFlights(this.source, this.destination, this.departureDate)
        .subscribe(
          (data: Flight[]) => {
            console.log('Available flights:', data);
            this.flights = data;
          },
          error => {
            console.error('Error fetching available flights:', error);
            this.flights = [];  // Reset flights if there's an error
            // this.errorMessage = 'Failed to fetch flights. Please try again later.';
          }
        );
    } else {
      console.error('Please fill in all fields');
      //this.errorMessage = 'Please fill in all fields.';
    }
  }

  navigateToMyHome(): void {
    this.router.navigate(['/home']);
  }

  onSourceChange(code: string): void {
    this.source = code;
  }

  onDestinationChange(code: string): void {
    this.destination = code;
  }


}
