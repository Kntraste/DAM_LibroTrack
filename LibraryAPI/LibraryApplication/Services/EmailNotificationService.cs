using System.Net;
using System.Net.Mail;

namespace LibraryApplication.Services;

public class EmailNotificationService : IEmailNotificationService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _username;
    private readonly string _password;

    public EmailNotificationService(string smtpServer, int smtpPort, string username, string password)
    {
        _smtpServer = smtpServer;
        _smtpPort = smtpPort;
        _username = username;
        _password = password;
    }

    public void SendEmail(string destination, string subject, string body)
    {
        var thread = new Thread(() =>
    {
        using var client = new SmtpClient(_smtpServer, _smtpPort);
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(_username, _password);
        client.Send(new MailMessage(_username, destination, subject, body));
    })
        {
            IsBackground = true
        };
        thread.Start();
    }
}
