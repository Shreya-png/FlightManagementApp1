using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBookingDemo4.Models
{
    public class Checkin
    {
        [Key]
        public int CheckinId { get; set; }

        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CheckinDate { get; set; }

        [Required]
        [RegularExpression("confirmed|pending", ErrorMessage = "Status must be either 'confirmed' or 'pending'.")]
        public string CheckinStatus { get; set; }

       


        // Navigation property for the Booking
        //public virtual Booking Booking { get; set; }
    }
}
