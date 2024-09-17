using System.ComponentModel.DataAnnotations;

namespace FlightBookingDemo4.DTOs
{
    public class UpdateBookingDto
    {
        [Required]
        public int BookingId { get; set; }

        [Required]
         public string Status { get; set; }
    }
}
