using System.ComponentModel.DataAnnotations;

namespace FlightBookingDemo4.Models.DTOs
{
    public class LoginDto
    {
        [Required]
        [MaxLength(15)]
        [RegularExpression(@"^\S*$", ErrorMessage = "Username should not contain spaces")]
        public string Username { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [MaxLength(10)]
        public string Password { get; set; }
    }
}
