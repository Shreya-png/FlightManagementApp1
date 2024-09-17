using FlightBookingDemo4.Models;
using FlightBookingDemo4.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FlightBookingDemo4.Services;
using System.Text;

namespace FlightBookingDemo4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly RegisterEmail _email;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext context, RegisterEmail email)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _email = email;
        }

        // Registration method to handle only "Passenger" registration
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
        {
            if (registrationDto == null)
                return BadRequest("Invalid client request");

            // Restrict registration to only passengers
            if (registrationDto.Role.ToLower() != "passenger")
            {
                return BadRequest("Only passengers can register.");
            }

            // Create IdentityUser
            var user = new IdentityUser
            {
                UserName = registrationDto.Username,
                Email = registrationDto.Email
            };

            var result = await _userManager.CreateAsync(user, registrationDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Add to the "Passenger" role
            if (!await _roleManager.RoleExistsAsync("Passenger"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Passenger"));
            }

            await _userManager.AddToRoleAsync(user, "Passenger");

            // Insert user into custom Users table
            var customUser = new Users
            {
                // Generate a new UserId for the Users table, assuming UserId is an auto-incrementing integer
                UserId=user.Id.ToString(),
                Name = registrationDto.Name,
                Email = registrationDto.Email,
                Phone = registrationDto.Phone,
                DateOfBirth = registrationDto.DateOfBirth,
                Username = registrationDto.Username, // Make sure this is not null
                Password = registrationDto.Password,
                Role = "Passenger"
            };

            _context.Users.Add(customUser);
            await _context.SaveChangesAsync();

            _email.SendEmail(registrationDto.Email, registrationDto.Username);

            return Ok(new { Message = "Passenger registered successfully" });
        }





        // Login method for both Admin and Passenger with JWT token generation
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
                return BadRequest("Invalid client request");

            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Unauthorized("Invalid credentials");

            // Fetch roles to determine whether the user is an Admin or Passenger
            var userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Contains("Admin") && !userRoles.Contains("Passenger"))
            {
                return Unauthorized("User is not authorized.");
            }

            var token = GenerateJwtToken(user);

            // Fetch custom user data from Users table
            var customUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            return Ok(new
            {
                Token = token,
                UserDetails = new
                {
                    customUser?.Name,
                    customUser?.Email,
                    customUser?.Phone,
                    customUser?.DateOfBirth
                }
            });
        }

        // Helper method to generate JWT token
        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            //var claims = new[]
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            //};

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
                //new Claim("UserId",user.Id)
                // new Claim("id","123") // Include UserId claim
            };

            var userRoles = _userManager.GetRolesAsync(user).Result;
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings.Issuer,
                Audience = jwtSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
