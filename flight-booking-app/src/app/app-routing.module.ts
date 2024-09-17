import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FlightBookingComponent } from './components/flight-booking/flight-booking.component'; // flight booking component
import { FlightSearchComponent } from './components/flight-search/flight-search.component'; // flight search component
import { BookingConfirmationComponent } from './components/booking-confirmation/booking-confirmation.component';
import { AdminComponent } from './components/admin/admin.component';
import { UserComponent } from './components/user/user.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { LoginComponent } from './components/login/login.component';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { AdminLoginComponent } from './components/admin-login/admin-login.component';
import { FlightSearchDummyComponent } from './components/flight-search-dummy/flight-search-dummy.component';
import { MyBookingsComponent } from './components/my-bookings/my-bookings.component';

const routes: Routes = [
  { path: 'user', component: UserComponent },
  { path: 'flight-search', component: FlightSearchComponent },
  { path: 'flight-booking/:id', component: FlightBookingComponent }, // Updated flight booking route
  { path: 'booking-confirmation/:bookingId', component: BookingConfirmationComponent },
  { path:  'admin', component:AdminComponent},
  { path: 'home',component:HomePageComponent},
  { path: 'login', component:LoginComponent},
  { path:'admin-dashboard', component:AdminDashboardComponent},
  { path:'admin-login',component:AdminLoginComponent},
  { path:'flight-search-dummy', component:FlightSearchDummyComponent},
  { path:'my-bookings',component:MyBookingsComponent},
  
  { path: '', redirectTo: '/flight-search-dummy', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
