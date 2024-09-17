

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightBookingDemo4.DTOs
{
    public class AddPassengersDto
    {
        [Required]
        public int BookingId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one passenger must be provided.")]
        [MaxLength(4, ErrorMessage = "You cannot add more than 4 passengers.")]
        public List<PassengerDto> Passengers { get; set; }
    }
}
