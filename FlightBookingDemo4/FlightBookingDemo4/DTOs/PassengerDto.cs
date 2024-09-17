using System;
using System.ComponentModel.DataAnnotations;

namespace FlightBookingDemo4.DTOs
{
    public class PassengerDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Age must be between 1 and 100.")]
        public int Age { get; set; }

        [Required]
        [MaxLength(10)]
        public string Gender { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"\+\d{2} \d{10}", ErrorMessage = "Phone number must be in the format +XX XXXXXXXXXX")]
        [MaxLength(20)]
        public string Phone { get; set; }
    }
}
