using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using FlightBookingDemo4.Models;
using FlightBookingDemo4.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace FlightBookingDemo4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Passenger")]
    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<IActionResult> CreatePayment(PaymentDto paymentDto)
        {
            // Fetch the booking by ID
            var booking = await _context.Bookings
                                        .FirstOrDefaultAsync(b => b.BookingId == paymentDto.BookingId);

            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            // Check if the booking status is 'confirmed'
            if (booking.Status != "confirmed")
            {
                return BadRequest("Payment cannot be processed because the booking status is not 'confirmed'.");
            }

            // Create a new payment based on the booking
            var payment = new Payment
            {
                BookingId = paymentDto.BookingId,
                Amount = booking.TotalPrice,
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = paymentDto.PaymentStatus
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Payment created successfully", paymentId = payment.PaymentId });
        }
    }
}
