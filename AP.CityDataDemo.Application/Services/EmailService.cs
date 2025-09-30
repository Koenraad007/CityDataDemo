using System.Net.Mail;

namespace AP.CityDataDemo.Application.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var gmailUser = "koenvanaken1999@gmail.com";
            var gmailPassword = "qozgcwizngbxnlby";

            using var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential(gmailUser, gmailPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage(gmailUser, to, subject, body);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}