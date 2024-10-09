using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string message);
}

public class EmailService : IEmailService
{
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _smtpPort = 465;
    private readonly string _senderEmail = "hwtechenterprisellc@gmail.com";  // Update with your email
    private readonly string _senderName = "HW Tech";  // Update with your name
    private readonly string _emailPassword = "hwkizgohhdssebjo";  // Secure this value in configuration

    public async Task SendEmailAsync(string toEmail, string subject, string messageText)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_senderName, _senderEmail));
        emailMessage.To.Add(new MailboxAddress("", toEmail));
        emailMessage.Subject = subject;

        emailMessage.Body = new TextPart("plain")
        {
            Text = messageText
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_smtpServer, _smtpPort, true);
            await client.AuthenticateAsync(_senderEmail, _emailPassword);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}