using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightBookingDemo4.Models
{
    public class Flight : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z]{3}[0-9]{3}$", ErrorMessage = "Incorrect format for Flight number.")]
        public string? FlightNumber { get; set; }

        [Required]
        public string? FlightName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Invalid Source Format.")]
        public string? Source { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Invalid Destination Format.")]
        public string? Destination { get; set; }

        [Required]
        public DateTime DepartureDate { get; set; }

        [Required]
        [RegularExpression(@"^([01]\d|2[0-3]):([0-5]\d):([0-5]\d)$", ErrorMessage = "Invalid Departure Time Format (HH:mm:ss).")]
        public string? DepartureTime { get; set; } // Changed to string

        [Required]
        public DateTime ArrivalDate { get; set; }

        [Required]
        [RegularExpression(@"^([01]\d|2[0-3]):([0-5]\d):([0-5]\d)$", ErrorMessage = "Invalid Arrival Time Format (HH:mm:ss).")]
        public string? ArrivalTime { get; set; } // Changed to string

        [Required]
        public decimal Fare { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Source == Destination)
            {
                yield return new ValidationResult("Source and Destination cannot be the same.", new[] { nameof(Source), nameof(Destination) });
            }

            if (!TimeSpan.TryParse(DepartureTime, out TimeSpan parsedDepartureTime))
            {
                yield return new ValidationResult("Invalid Departure Time format.", new[] { nameof(DepartureTime) });
            }

            if (!TimeSpan.TryParse(ArrivalTime, out TimeSpan parsedArrivalTime))
            {
                yield return new ValidationResult("Invalid Arrival Time format.", new[] { nameof(ArrivalTime) });
            }

            DateTime departureDateTime = DepartureDate.Date + parsedDepartureTime;
            DateTime arrivalDateTime = ArrivalDate.Date + parsedArrivalTime;

            if (arrivalDateTime <= departureDateTime)
            {
                yield return new ValidationResult("Arrival date and time must be later than departure date and time.", new[] { nameof(ArrivalDate), nameof(ArrivalTime) });
            }
        }
    }
}
