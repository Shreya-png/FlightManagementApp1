using FlightBookingDemo4.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Payment
{
    [Key]
    public int PaymentId { get; set; }  // Identity column

    [ForeignKey("Booking")]
    public int BookingId { get; set; }  // Foreign key to Booking


    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }


    [DataType(DataType.DateTime)]
    public DateTime PaymentDate { get; set; }

    [Required]
    [RegularExpression("(completed|pending|failed)", ErrorMessage = "Payment status must be 'completed', 'pending', or 'failed'.")]
    public string PaymentStatus { get; set; }

    public Booking Booking { get; set; }  // Navigation property
}

