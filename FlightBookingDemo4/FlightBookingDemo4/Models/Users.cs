using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightBookingDemo4.Models
{
    public class Users : IValidatableObject
    {
        [Key]
        public string UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"\+\d{2} \d{10}", ErrorMessage = "Phone number must be in the format +XX XXXXXXXXXX")]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(15)]
        [RegularExpression(@"^\S*$", ErrorMessage = "Username should not contain spaces")]
        public string Username { get; set; }

        [Required]
        //[MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        //[MaxLength(8)]
        public string Password { get; set; }

        // New Role attribute
        [Required]
        [RegularExpression(@"^(Admin|Passenger)$", ErrorMessage = "Role must be either 'Admin' or 'Passenger'.")]
        public string Role { get; set; } // Role as a string, allowing only "Admin" or "Passenger"

        // Validation to ensure DateOfBirth is not in the future
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateOfBirth > DateTime.Today)
            {
                yield return new ValidationResult("Date of birth cannot be in the future.", new[] { nameof(DateOfBirth) });
            }
        }
    }
}
