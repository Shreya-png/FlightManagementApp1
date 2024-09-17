
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using FlightBookingDemo4.Models;
using FlightBookingDemo4.Services;
using FlightBookingDemo4.DTOs;
using System.IdentityModel.Tokens.Jwt;



namespace FlightBookingDemo4.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "Passenger")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly BookingEmail _email;
        //private readonly IUserContext _userContext;

        public BookingsController(ApplicationDbContext context, BookingEmail email, IUserContext userContext)
        {
            _context = context;
            _email = email;
            //_userContext = userContext;
        }



        // GET: api/Bookings/Flights
        [HttpGet("Available_Flights")]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlightsBySourceDestinationAndDate(string source, string destination, DateTime departureDate)
        {
            var flights = await _context.Flights
                .Where(f => f.Source == source.ToUpper() &&
                f.Destination == destination.ToUpper() &&
                f.DepartureDate.Date == departureDate.Date)
                .ToListAsync();

            if (!flights.Any())
            {
                return NotFound("No flights available for the selected source, destination, and departure date.");
            }

            return Ok(flights);
        }


        // GET: api/Bookings/Flights
        [HttpGet("CheapestFlight")]
        public async Task<ActionResult<Flight>> GetCheapestFlightBySourceDestinationAndMonth(
            [FromQuery] string source,
            [FromQuery] string destination,
            [FromQuery] DateTime monthStartDate)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(destination))
            {
                return BadRequest("Source and destination cannot be empty.");
            }

            // Define the start and end of the month
            var monthEndDate = monthStartDate.AddMonths(1).AddDays(-1);

            // Fetch the cheapest flight for the provided source, destination, and within the specified month
            var flight = await _context.Flights
                .Where(f => f.Source == source.ToUpper() &&
                            f.Destination == destination.ToUpper() &&
                            f.DepartureDate.Date >= monthStartDate.Date &&
                            f.DepartureDate.Date <= monthEndDate.Date)
                .OrderBy(f => f.Fare) // Order by fare to get the lowest price first
                .FirstOrDefaultAsync(); // Select only the cheapest flight

            if (flight == null)
            {
                return NotFound("No flights available for the selected source, destination, and month.");
            }

            return Ok(flight);
        }








        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookingResponse>> PostBooking([FromBody] BookingDto bookingDto)
        {
            // Check if the flight exists
            var flight = await _context.Flights
                .Where(f => f.Id == bookingDto.FlightId)
                .FirstOrDefaultAsync();

            if (flight == null)
            {
                return NotFound("The specified flight does not exist.");
            }

            // Calculate the total price based on fare and number of seats
            var totalPrice = flight.Fare * bookingDto.NumberOfSeats;

            // Create the booking entity from the DTO
            var booking = new Booking
            {
                FlightId = bookingDto.FlightId,
                UserId = bookingDto.UserId,
                NumberOfSeats = bookingDto.NumberOfSeats,
                TotalPrice = totalPrice,
                BookingDate = DateTime.UtcNow,
                Status = bookingDto.Status
            };

            // Add the booking to the context
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(booking.UserId);
            if (user != null)
            {
                // Send email with user details and booking details
                _email.SendEmail(user.Email, user.Name, booking); // Now passing the booking object as well
            }

            

            // Create the response object
            var response = new BookingResponse
            {
                Booking = new BookingDto
                {
                    BookingId = booking.BookingId, // Now includes the generated bookingId
                    UserId = booking.UserId,
                    FlightId = booking.FlightId,
                    NumberOfSeats = booking.NumberOfSeats,
                    TotalPrice = booking.TotalPrice, // The calculated total price
                    //BookingDate = booking.BookingDate.ToString("yyyy-MM-dd"),
                    Status = booking.Status // The updated status
                },
                Flight = flight
            };

            // Return a simple 201 Created response with the booking data
            return StatusCode(StatusCodes.Status201Created, response);
        }


        [HttpPost("AddPassengers")]
public async Task<ActionResult> AddPassengers([FromBody] AddPassengersDto addPassengersDto)
{
    var booking = await _context.Bookings
        .Include(b => b.Passengers)
        .FirstOrDefaultAsync(b => b.BookingId == addPassengersDto.BookingId);

    if (booking == null)
    {
        return NotFound(new { message = "The specified booking does not exist." });
    }

    foreach (var passengerDto in addPassengersDto.Passengers)
    {
        var passenger = new Passenger
        {
            UserId = passengerDto.UserId,
            Name = passengerDto.Name,
            Age = passengerDto.Age,
            Gender = passengerDto.Gender,
            Email = passengerDto.Email,
            Phone = passengerDto.Phone,
            BookingId = addPassengersDto.BookingId
        };

        _context.Passengers.Add(passenger);
    }

    await _context.SaveChangesAsync();
    return Ok(new { message = "Passengers added successfully." });
}



[HttpPut("Edit_by_Booking_Id")]
public async Task<IActionResult> PutBooking([FromBody] UpdateBookingDto updateDto)
{
    if (updateDto == null || updateDto.BookingId == 0)
    {
        return BadRequest("Booking ID is required.");
    }

    // Fetch the existing booking
    var existingBooking = await _context.Bookings
        .FirstOrDefaultAsync(b => b.BookingId == updateDto.BookingId);

    if (existingBooking == null)
    {
        return NotFound("Booking not found.");
    }

    // Update only the status field
    existingBooking.Status = updateDto.Status;

    // Mark the entity as modified
    _context.Entry(existingBooking).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!BookingExists(updateDto.BookingId))
        {
            return NotFound("Booking not found.");
        }
        else
        {
            throw;
        }
    }

    return NoContent();
}



        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // GET: api/Bookings?userId=5
[HttpGet("DisplayConfirmedBookingsByUserId")]
public async Task<ActionResult<IEnumerable<BookingResponse>>> GetBookings([FromQuery] string userId)
{
    // Validate the userId
    if (string.IsNullOrEmpty(userId))
    {
        return BadRequest("Invalid user ID.");
    }

    // Fetch confirmed bookings for the provided user ID
    var bookings = await _context.Bookings
        .Where(b => b.UserId == userId && b.Status == "confirmed")
        .ToListAsync();

    if (bookings == null || !bookings.Any())
    {
        return NotFound("No confirmed bookings found for the specified user.");
    }

    // Create a list of BookingResponse objects
    var bookingResponses = new List<BookingResponse>();

    foreach (var booking in bookings)
    {
        // Fetch the associated flight details
        var flight = await _context.Flights.FindAsync(booking.FlightId);
        
        if (flight == null)
        {
            return NotFound($"Flight with ID {booking.FlightId} not found.");
        }

        var totalPrice = booking.NumberOfSeats * flight.Fare;

        // Add the BookingResponse to the list
        bookingResponses.Add(new BookingResponse
        {
            Booking = new BookingDto
            {
                BookingId = booking.BookingId,
                UserId = booking.UserId,
                FlightId = booking.FlightId,
                NumberOfSeats = booking.NumberOfSeats,
                Status = booking.Status,
                TotalPrice = totalPrice
            },
            Flight = flight
        });
    }

    return Ok(bookingResponses);
}



        [HttpGet("DisplayCancelledBookingsByUserId")]
public async Task<ActionResult<IEnumerable<Booking>>> GetCancelledBookings([FromQuery] string userId)
{
    // Validate the userId
    if (string.IsNullOrEmpty(userId))
    {
        return BadRequest("Invalid user ID.");
    }

    // Fetch bookings for the provided user ID with status 'cancelled'
    var bookings = await _context.Bookings
        .Where(b => b.UserId == userId && b.Status == "cancelled")
        .ToListAsync();

    if (bookings == null || !bookings.Any())
    {
        return NotFound("No cancelled bookings found for the specified user.");
    }

    return Ok(bookings);
}




        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }

        ////[HttpGet("debug")]
        //public IActionResult DebugClaims()
        //{
        //    var claims = User.Claims.Select(c => new { c.Type, c.Value });
        //    return Ok(claims);
        //}



        //// GET: api/Bookings
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        //{
        //    return await _context.Bookings.ToListAsync();
        //}

        [HttpGet("GetBookingsByUserId")]
public async Task<ActionResult<IEnumerable<BookingResponse>>> GetBookingsByUserId([FromQuery] string userId)
{
    // Validate the userId
    if (string.IsNullOrEmpty(userId))
    {
        return BadRequest("Invalid user ID.");
    }

    // Fetch bookings for the provided user ID with status 'confirmed' or 'cancelled'
    var bookings = await _context.Bookings
        .Where(b => b.UserId == userId && (b.Status == "confirmed" || b.Status == "cancelled"))
        .ToListAsync();

    if (bookings == null || !bookings.Any())
    {
        return NotFound("No confirmed or cancelled bookings found for the specified user.");
    }

    // Create a list of BookingResponse objects
    var bookingResponses = new List<BookingResponse>();

    foreach (var booking in bookings)
    {
        // Fetch the associated flight details
        var flight = await _context.Flights.FindAsync(booking.FlightId);
        
        if (flight == null)
        {
            return NotFound($"Flight with ID {booking.FlightId} not found.");
        }

        var totalPrice = booking.NumberOfSeats * flight.Fare;

        // Add the BookingResponse to the list
        bookingResponses.Add(new BookingResponse
        {
            Booking = new BookingDto
            {
                BookingId = booking.BookingId,
                UserId = booking.UserId,
                FlightId = booking.FlightId,
                NumberOfSeats = booking.NumberOfSeats,
                Status = booking.Status,
                TotalPrice = totalPrice
            },
            Flight = flight
        });
    }

    return Ok(bookingResponses);
}

[HttpGet("user-bookings")]
public async Task<IActionResult> GetBookingsForCurrentUser()
{
    try
    {
        // Extract JWT Token from the Authorization header
        var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);

        // Extract the User ID from the token
        var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == "nameid");
        if (userIdClaim == null)
        {
            return BadRequest("User ID not found in token.");
        }

        var userId = userIdClaim.Value;

        // Fetch bookings for the current user
        var bookings = await _context.Bookings
            .Where(b => b.UserId == userId)
            .Include(b => b.Passengers) // Include passengers
            .Include(b => b.Flight) // Include flight
            .ToListAsync();

        if (bookings == null || !bookings.Any())
        {
            return NotFound("No bookings found for the current user.");
        }

        // Create the response list
        var bookingResponses = new List<BookingResponse>();

        foreach (var booking in bookings)
        {
            var flight = booking.Flight; // Assuming Flight is already included
            var passengers = booking.Passengers.Select(p => new PassengerDto
            {
                UserId = p.UserId,
                Name = p.Name,
                Age = p.Age,
                Gender = p.Gender,
                Email = p.Email,
                Phone = p.Phone
            }).ToList();

            var bookingResponse = new BookingResponse
            {
                Booking = new BookingDto
                {
                    BookingId = booking.BookingId,
                    UserId = booking.UserId,
                    FlightId = booking.FlightId,
                    NumberOfSeats = booking.NumberOfSeats,
                    Status = booking.Status,
                    TotalPrice = booking.TotalPrice
                },
                Flight = flight,
                Passengers = passengers
            };

            bookingResponses.Add(bookingResponse);
        }

        return Ok(bookingResponses);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}



    // GET: api/Bookings/BookingDetails/{bookingId}
[HttpGet("BookingDetails/{bookingId}")]
public async Task<ActionResult<BookingResponse>> GetBookingDetailsById(int bookingId)
{
    // Fetch the booking based on the BookingId
    var booking = await _context.Bookings
        .Include(b => b.Passengers) // Include the passengers if you need to return passenger details as well
        .FirstOrDefaultAsync(b => b.BookingId == bookingId);

    if (booking == null)
    {
        return NotFound($"Booking with ID {bookingId} not found.");
    }

    // Fetch the associated flight details
    var flight = await _context.Flights.FindAsync(booking.FlightId);
    if (flight == null)
    {
        return NotFound($"Flight with ID {booking.FlightId} not found.");
    }

    // Calculate the total price (if needed)
    var totalPrice = booking.NumberOfSeats * flight.Fare;

    // Create the response object
    var bookingResponse = new BookingResponse
    {
        Booking = new BookingDto
        {
            BookingId = booking.BookingId,
            UserId = booking.UserId,
            FlightId = booking.FlightId,
            NumberOfSeats = booking.NumberOfSeats,
            TotalPrice = totalPrice,
            Status = booking.Status
        },
        Flight = flight,
        Passengers = booking.Passengers.Select(p => new PassengerDto
        {
            UserId = p.UserId,
            Name = p.Name,
            Age = p.Age,
            Gender = p.Gender,
            Email = p.Email,
            Phone = p.Phone
        }).ToList()
    };

    return Ok(bookingResponse);
}



    }


    
    
}
