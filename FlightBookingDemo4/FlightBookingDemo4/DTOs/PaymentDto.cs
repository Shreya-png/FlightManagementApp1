
using System.ComponentModel.DataAnnotations;

namespace FlightBookingDemo4.DTOs
{
    public class PaymentDto
    {
        [Required]
        public int BookingId { get; set; }

        [Required]
        [RegularExpression("(completed|pending|failed)", ErrorMessage = "Payment status must be 'completed', 'pending', or 'failed'.")]
        public string PaymentStatus { get; set; }
    }


}
