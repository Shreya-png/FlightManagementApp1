using FlightBookingDemo4.DTOs;
namespace FlightBookingDemo4.Models
{
    public class BookingResponse
    {
        public BookingDto Booking { get; set; }
        public Flight Flight { get; set; }

        public List<PassengerDto> Passengers { get; set; } 
    }
}
