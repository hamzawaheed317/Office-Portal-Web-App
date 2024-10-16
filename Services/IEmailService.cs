using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string message);
}

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _senderEmail;
    private readonly string _senderName;
    private readonly string _emailPassword;

    public EmailService(string smtpServer, int smtpPort, string senderEmail, string senderName, string emailPassword)
    {
        _smtpServer = smtpServer;
        _smtpPort = smtpPort;
        _senderEmail = senderEmail;
        _senderName = senderName;
        _emailPassword = emailPassword;
    }

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
