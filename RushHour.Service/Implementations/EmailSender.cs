namespace RushHour.Service.Implementations
{
    using Contracts;
    using Microsoft.Extensions.Configuration;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;

        public EmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (var client = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = configuration["Email:Username"],
                    Password = configuration["Email:Password"]
                };

                client.Credentials = credential;
                client.Host = configuration["Email:Host"];
                client.Port = int.Parse(configuration["Email:Port"]);
                client.EnableSsl = true;

                using (var emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(email));
                    emailMessage.From = new MailAddress(configuration["Email:Email"]);
                    emailMessage.Subject = subject;
                    emailMessage.Body = message;
                    client.Send(emailMessage);
                }
            }
            await Task.CompletedTask;
        }
    }
}
