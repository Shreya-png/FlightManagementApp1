using System;
using System.ComponentModel.DataAnnotations;

namespace FlightBookingDemo4.DTOs
{
    public class BookingDto
    {
        public int BookingId { get; set; }
        [Required]
        public int FlightId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Range(1, 4, ErrorMessage = "The number of seats must be between 1 and 4.")]
        public int NumberOfSeats { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        // Optional: you might include other fields if necessary, such as booking date or status
        [Required]
        public string Status { get; set; }

        //public List<PassengerDto> Passengers { get; set; }

        
    }
}
