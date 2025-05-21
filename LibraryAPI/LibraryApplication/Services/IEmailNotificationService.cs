namespace LibraryApplication.Services;

public interface IEmailNotificationService
{
    void SendEmail(string destination, string subject, string body);
}
