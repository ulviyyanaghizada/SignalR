using Pronia.Abstractions.Services;
using System.Net;
using System.Net.Mail;

namespace Pronia.Service
{
    public class EmailService:IEmailService
    {
        IConfiguration _configuration { get; }

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(string mailTo, string subject, string body,bool isBodyHtml = false)
        {
            using SmtpClient smtp = new SmtpClient(_configuration["Email:Host"],
                Convert.ToInt32(_configuration["Email:Port"]));

            smtp.EnableSsl= true;
            smtp.Credentials = new NetworkCredential(_configuration["Email:Login"],
               _configuration["Email:Password"]);

            MailAddress from = new MailAddress(_configuration["Email:Login"], "SABAH Groups");
            MailAddress to = new MailAddress(mailTo); //"Ccefermustafayev@gmail.com"

            using MailMessage message = new MailMessage(from, to);

            message.Subject = subject; //"Your Account is not Safe..."
            message.Body = body;   //"We offer you"
            message.IsBodyHtml= isBodyHtml;

            smtp.Send(message);

        }
    }
}
