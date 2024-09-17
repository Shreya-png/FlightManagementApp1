using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBookingDemo4.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [ForeignKey("Flight")]
        public int FlightId { get; set; }

        [ForeignKey("Users")]
        public string UserId { get; set; } 

        [Range(1, 4, ErrorMessage = "The number of seats must be between 1 and 4.")]
        public int NumberOfSeats { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime BookingDate { get; set; }

        [Required]
        [RegularExpression("(confirmed|cancelled)", ErrorMessage = "Status must be either 'confirmed' or 'cancelled'")]
        public string Status { get; set; }

        public virtual Flight Flight { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

        public ICollection<Passenger> Passengers { get; set; } = new List<Passenger>();


    }

}
