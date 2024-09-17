using System.Net.Mail;
using System.Net;
using System.Text;

namespace FlightBookingDemo4.Services
{
    public class RegisterEmail
    {
        public void SendEmail(string toEmail, string username)
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
            mailMessage.Subject = "Regarding to Flight Booking Application";
            mailMessage.IsBodyHtml = true;
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>User Registered</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat($"Dear {username}");
            mailBody.AppendFormat("<p>Thank you For Registering account</p>");
            mailMessage.Body = mailBody.ToString();

            // Send email
            client.Send(mailMessage);
        }
    }
}