import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule,  HTTP_INTERCEPTORS ,provideHttpClient, withFetch } from '@angular/common/http';  // Make sure withFetch is imported
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { FlightSearchComponent } from './components/flight-search/flight-search.component';
import { FlightBookingComponent } from './components/flight-booking/flight-booking.component';
import { BookingConfirmationComponent } from './components/booking-confirmation/booking-confirmation.component';
import { AdminComponent } from './components/admin/admin.component';
import { UserComponent } from './components/user/user.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { UserNavbarComponent } from './components/user-navbar/user-navbar.component';
import { FlightSearchDummyComponent } from './components/flight-search-dummy/flight-search-dummy.component';
import { AdminLoginComponent } from './components/admin-login/admin-login.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MyBookingsComponent } from './components/my-bookings/my-bookings.component';





@NgModule({
  declarations: [
    AppComponent,
    FlightSearchComponent,
    FlightBookingComponent,
    BookingConfirmationComponent,
    AdminComponent,
    UserComponent,
    HomePageComponent,
    NavbarComponent,
    LoginComponent,
    RegisterComponent,
    AdminDashboardComponent,
    UserNavbarComponent,
    AdminLoginComponent,
    FlightSearchDummyComponent,
    MyBookingsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule
    
  ],
  providers: [
    provideClientHydration(),
    provideHttpClient(withFetch()),
    provideAnimationsAsync()  // Add withFetch here
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
