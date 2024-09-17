using System.Net.Mail;
using System.Net;
using System.Text;
using FlightBookingDemo4.Models; // Import the Booking model

namespace FlightBookingDemo4.Services
{
    public class BookingEmail
    {
        public void SendEmail(string toEmail, string username, Booking booking)
        {
            // Set up SMTP client
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("shreyachakraborty580@gmail.com", "fjxv mmjn xhcl npjr");

            // Create email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("shreyachakraborty580@gmail.com");
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = "Flight Booking Confirmation";
            mailMessage.IsBodyHtml = true;

            // Ensure the Flight property is loaded
            //if (booking.Flight == null)
            //{
            //    throw new InvalidOperationException("Flight details are missing from the booking.");
            //}

            // Build the email body with booking details
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>Booking Confirmed</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat($"Dear {username},");
            mailBody.AppendFormat("<p>Thank you for booking with us!</p>");
            mailBody.AppendFormat("<p>Your booking details are as follows:</p>");
            mailBody.AppendFormat("<ul>");
            mailBody.AppendFormat($"<li>Booking ID: {booking.BookingId}</li>");
            mailBody.AppendFormat($"<li>Flight Number: {booking.FlightId}</li>");
           
            mailBody.AppendFormat($"<li>Number of Seats: {booking.NumberOfSeats}</li>");
            mailBody.AppendFormat($"<li>Total Price: {booking.TotalPrice:C}</li>");
            mailBody.AppendFormat($"<li>Booking Date: {booking.BookingDate.ToString("f")}</li>");
            mailBody.AppendFormat($"<li>Status: {booking.Status}</li>");
            mailBody.AppendFormat("</ul>");
            mailBody.AppendFormat("<p>We look forward to serving you!</p>");
            mailBody.AppendFormat("<br />Best regards,<br />Flight Booking Team");

            mailMessage.Body = mailBody.ToString();

            // Send email
            client.Send(mailMessage);
        }

    }
}
