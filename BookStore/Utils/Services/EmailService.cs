using BookStore.Utils.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace BookStore.Utils.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _client;

        public EmailService()
        {
            _client = new SmtpClient();
        }

        public void Dispose()
        {
            _client.Disconnect(true);
            _client.Dispose();
        }

        public async Task SendTextEmailAsync(string to, string subject, string text)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Notification from Book Store (don't reply)", "some@mail.com"));
            email.To.Add(new MailboxAddress(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Text) { Text = text };

            await InternalSendMessageAsync(email);
        }

        private async Task InternalSendMessageAsync(MimeMessage email)
        {
            if (!_client.IsConnected)
                await _client.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);

            if (!_client.IsAuthenticated)
                await _client.AuthenticateAsync("username", "password");
            
            await _client.SendAsync(email);
        }
    }
}