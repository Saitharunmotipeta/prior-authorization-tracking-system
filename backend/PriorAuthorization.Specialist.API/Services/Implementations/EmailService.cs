namespace PriorAuthorization.Specialist.API.Services.Implementations
{
    using MailKit.Net.Smtp;
    using MimeKit;
    using PriorAuthorization.Specialist.API.Services.Interfaces;

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAsync(
            string to,
            string subject,
            string body)
        {
            var email = new MimeMessage();

            email.From.Add(
                MailboxAddress.Parse(
                    _configuration["EmailSettings:SenderEmail"]));

            email.To.Add(
                MailboxAddress.Parse(to));

            email.Subject = subject;

            email.Body =
                new TextPart("plain")
                {
                    Text = body
                };

            using var smtp =
                new SmtpClient();

            await smtp.ConnectAsync(
                _configuration["EmailSettings:SmtpServer"],
                int.Parse(
                    _configuration["EmailSettings:Port"]),
                MailKit.Security.SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _configuration["EmailSettings:Username"],
                _configuration["EmailSettings:Password"]);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}
