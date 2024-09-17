using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using FlightBookingDemo4.Models;
using Microsoft.AspNetCore.Authorization;

namespace FlightBookingDemo4.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FlightsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Flights
        [HttpGet("Displaying List of all Flights")]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
        {
            return await _context.Flights.ToListAsync();
        }


        // GET: api/Flights/bySourceDestination
        [HttpGet("Display Flights by Source & Destination")]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlightsBySourceDestination(string source, string destination)
        {
            var flights = await _context.Flights
                .Where(f => f.Source == source.ToUpper() && f.Destination == destination.ToUpper())
                .ToListAsync();

            if (flights == null || flights.Count == 0)
            {
                return NotFound("No flights found for the given source and destination.");
            }

            return flights;
        }

        

        // POST: api/Flights
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Flight>> PostFlight(Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetFlight", new { id = flight.Id }, flight);
            return Ok(flight);
        }




        // PUT: api/Flights/bySourceDestinationAndFlightNumber
        [HttpPut("Edit by Source, Destination & FlightNumber")]
        public async Task<IActionResult> PutFlightBySourceDestinationAndNumber(string source, string destination, string flightNumber, Flight updatedFlight)
        {
            var flight = await _context.Flights
                .FirstOrDefaultAsync(f => f.Source == source.ToUpper() &&
                                          f.Destination == destination.ToUpper() &&
                                          f.FlightNumber == flightNumber);

            if (flight == null)
            {
                return NotFound("No flight found for the given source, destination, and flight number.");
            }

            // Update the matching flight
            flight.FlightName = updatedFlight.FlightName;
            flight.DepartureDate = updatedFlight.DepartureDate;
            flight.DepartureTime = updatedFlight.DepartureTime;
            flight.ArrivalDate = updatedFlight.ArrivalDate;
            flight.ArrivalTime = updatedFlight.ArrivalTime;
            flight.Fare = updatedFlight.Fare;
            // Add other properties that need to be updated here

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating flight.");
            }

            return NoContent();
        }


        

        // DELETE: api/Flights/byFlightNumber/{flightNumber}
        [HttpDelete("by Flight Number")]
        public async Task<IActionResult> DeleteFlightByNumber(string flightNumber)
        {
            // Find the flight by FlightNumber
            var flight = await _context.Flights
                .FirstOrDefaultAsync(f => f.FlightNumber == flightNumber);

            if (flight == null)
            {
                return NotFound("Flight not found for the given flight number.");
            }

            // Remove the flight
            _context.Flights.Remove(flight);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting flight.");
            }

            return NoContent();
        }



        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.Id == id);
        }



        //// GET: api/Flights/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Flight>> GetFlight(int id)
        //{
        //    var flight = await _context.Flights.FindAsync(id);

        //    if (flight == null)
        //    {
        //        return NotFound();
        //    }

        //    return flight;
        //}

        // PUT: api/Flights/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutFlight(int id, Flight flight)
        //{
        //    if (id != flight.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(flight).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!FlightExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


    }
}
