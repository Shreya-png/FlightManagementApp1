import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service'; // Adjust the path as necessary
import { Flight } from '../../models/flight.model';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  flights: Flight[] = [];
  selectedFlight: Flight = this.initializeFlight();
  newFlight: Flight = this.initializeFlight();
  isEditing: boolean = false;
  hideNavbarButtons: boolean = true;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.getFlights();
  }

  initializeFlight(): Flight {
    return {
      id: 0,
      flightNumber: '',
      flightName: '',
      source: '',
      destination: '',
      departureDate: '',
      departureTime: '',
      arrivalDate: '',
      arrivalTime: '',
      fare: 0
    };
  }

  getFlights(): void {
    this.userService.getFlights().subscribe(
      (data: Flight[]) => {
        this.flights = data;
      },
      (error) => {
        console.error('Error fetching flights', error);
      }
    );
  }

  editFlight(flight: Flight): void {
    this.selectedFlight = { ...flight };
    this.isEditing = true;
  }

  // updateFlight(): void {
  //   if (this.selectedFlight) {
  //     this.userService.updateFlightBySourceDestinationAndNumber(
  //       this.selectedFlight.source,
  //       this.selectedFlight.destination,
  //       this.selectedFlight.flightNumber,
  //       this.selectedFlight
  //     ).subscribe(
  //       () => {
  //         this.getFlights();
  //         this.cancelEdit();
  //       },
  //       (error) => {
  //         console.error('Error updating flight', error);
  //       }
  //     );
  //   }
  // }

  updateFlight(): void {
    if (this.selectedFlight) {
      this.selectedFlight.departureDate = this.formatDateToYYYYMMDD(this.selectedFlight.departureDate);
      this.selectedFlight.departureTime = this.formatTimeToHHMMSS(this.selectedFlight.departureTime);
      this.selectedFlight.arrivalDate = this.formatDateToYYYYMMDD(this.selectedFlight.arrivalDate);
      this.selectedFlight.arrivalTime = this.formatTimeToHHMMSS2(this.selectedFlight.arrivalTime);

      this.userService.updateFlightBySourceDestinationAndNumber(
        this.selectedFlight.source,
        this.selectedFlight.destination,
        this.selectedFlight.flightNumber,
        this.selectedFlight
      ).subscribe(
        () => {
          this.getFlights();
          this.cancelEdit();
        },
        (error) => {
          console.error('Error updating flight', error);
        }
      );
    }
  }



  
  addFlight(): void {
    this.newFlight.departureDate = this.formatDateToYYYYMMDD(this.newFlight.departureDate);
    this.newFlight.departureTime=this.formatTimeToHHMMSS(this.newFlight.departureTime);
    this.newFlight.arrivalDate = this.formatDateToYYYYMMDD(this.newFlight.arrivalDate);
    this.newFlight.arrivalTime=this.formatTimeToHHMMSS2(this.newFlight.arrivalTime);
  
    this.userService.addFlight(this.newFlight).subscribe(
      (flight: Flight) => {
        this.flights.push(flight);
        this.resetNewFlight();
      },
      (error) => {
        console.error('Error adding flight', error);
      }
    );
  }
  

  deleteFlight(flightNumber: string): void {
    if (confirm(`Are you sure you want to delete flight number ${flightNumber}?`)) {
      this.userService.deleteFlightByNumber(flightNumber).subscribe(
        () => {
          this.flights = this.flights.filter(f => f.flightNumber !== flightNumber);
        },
        (error) => {
          console.error('Error deleting flight', error);
        }
      );
    }
  }

  cancelEdit(): void {
    this.selectedFlight = this.initializeFlight();
    this.isEditing = false;
  }

  resetNewFlight(): void {
    this.newFlight = this.initializeFlight();
  }

  formatDateToYYYYMMDD(date: string): string {
    const parsedDate = new Date(date);
    const year = parsedDate.getFullYear();
    const month = ('0' + (parsedDate.getMonth() + 1)).slice(-2);
    const day = ('0' + parsedDate.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
  }
  
//   formatTimeToHHMMSS(departureTime: string): string {
//     if (!departureTime) return '00:00:00'; // Return a default value if timeString is empty

//     // Append ":00" to the time string to represent seconds
//     return `${departureTime}:00`;
// }

// formatTimeToHHMMSS2(arrivalTime: string): string {
//   if (!arrivalTime) return '00:00:00'; // Return a default value if timeString is empty

//   // Append ":00" to the time string to represent seconds
//   return `${arrivalTime}:00`;
// }

formatTimeToHHMMSS(time: string): string {
  if (!time) return '00:00:00'; // Return a default value if time is empty

  // Ensure the time is in HH:mm format
  const [hours, minutes] = time.split(':');
  return `${hours}:${minutes}:00`;
}

formatTimeToHHMMSS2(time: string): string {
  if (!time) return '00:00:00'; // Return a default value if time is empty

  // Ensure the time is in HH:mm format
  const [hours, minutes] = time.split(':');
  return `${hours}:${minutes}:00`;
}

}
