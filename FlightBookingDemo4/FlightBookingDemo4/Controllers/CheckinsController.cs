using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightBookingDemo4.Models;
using Microsoft.AspNetCore.Authorization;

namespace FlightBookingDemo4.Controllers
{
    [Route("api/[controller]")]
    // [Authorize(Roles = "Passenger")]
    [ApiController]
    public class CheckinsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CheckinsController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: api/Checkins/byUser/5
        [HttpGet("by User ID")]
        public async Task<ActionResult<IEnumerable<Checkin>>> GetCheckinByUserId(string userId)
        {
            var checkins = await (from c in _context.Checkins
                                  join b in _context.Bookings on c.BookingId equals b.BookingId
                                  where b.UserId == userId
                                  select c).ToListAsync();

            if (checkins == null || !checkins.Any())
            {
                return NotFound();
            }

            return checkins;
        }



        // PUT: api/Checkins/byUser/5
        [HttpPut("Edit by User ID")]
        public async Task<IActionResult> PutCheckinByUserId(string userId, Checkin checkin)
        {
            // Find the booking associated with the checkin and userId
            var existingCheckin = await (from c in _context.Checkins
                                         join b in _context.Bookings on c.BookingId equals b.BookingId
                                         where b.UserId == userId && c.CheckinId == checkin.CheckinId
                                         select c).FirstOrDefaultAsync();

            if (existingCheckin == null)
            {
                return NotFound("Checkin not found for the specified user.");
            }

            // Update the fields that are allowed to be updated
            existingCheckin.CheckinStatus = checkin.CheckinStatus;
            existingCheckin.CheckinDate = checkin.CheckinDate;

            // Mark the entity as modified
            _context.Entry(existingCheckin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckinExists(existingCheckin.CheckinId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/Checkins/EditByBookingId/{bookingId}
[HttpPut("EditByBookingId/{bookingId}")]
public async Task<IActionResult> PutCheckinByBookingId(int bookingId, Checkin updatedCheckin)
{
    // Find the check-in associated with the provided booking ID
    var existingCheckin = await _context.Checkins
        .FirstOrDefaultAsync(c => c.BookingId == bookingId);

    if (existingCheckin == null)
    {
        return NotFound("Check-in not found for the specified booking ID.");
    }

    // Update the allowed fields
    existingCheckin.CheckinStatus = updatedCheckin.CheckinStatus;
    existingCheckin.CheckinDate = updatedCheckin.CheckinDate;

    // Mark the entity as modified
    _context.Entry(existingCheckin).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!CheckinExists(existingCheckin.CheckinId))
        {
            return NotFound("Check-in no longer exists.");
        }
        else
        {
            throw;
        }
    }

    return NoContent();
}



        // POST: api/Checkins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Checkin>> PostCheckin(Checkin checkin)
        {
            _context.Checkins.Add(checkin);
            await _context.SaveChangesAsync();

            return Ok(checkin);
        }

        // DELETE: api/Checkins/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCheckin(int id)
        //{
        //    var checkin = await _context.Checkins.FindAsync(id);
        //    if (checkin == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Checkins.Remove(checkin);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool CheckinExists(int id)
        {
            return _context.Checkins.Any(e => e.CheckinId == id);
        }
    }
}
