using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace com.teamseven.musik.be.Services.Authentication
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            var fromAddress = new MailAddress(_configuration["EmailSettings:Username"], "Team Seven");
            var toAddress = new MailAddress(toEmail);
            string fromPassword = _configuration["EmailSettings:Password"];

            var smtp = new SmtpClient
            {
                Host = _configuration["EmailSettings:Host"],
                Port = int.Parse(_configuration["EmailSettings:Port"]),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
