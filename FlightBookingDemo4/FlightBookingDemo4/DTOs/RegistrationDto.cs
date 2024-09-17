using System.ComponentModel.DataAnnotations;

namespace FlightBookingDemo4.Models.DTOs
{
    public class RegistrationDto
    {
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
        //[MaxLength(10)]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"^(Admin|Passenger)$", ErrorMessage = "Role must be either 'Admin' or 'Passenger'.")]
        public string Role { get; set; }
    }
}
