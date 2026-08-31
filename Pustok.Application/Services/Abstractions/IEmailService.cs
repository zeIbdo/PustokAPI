namespace Pustok.Application.Services.Abstractions;

public interface IEmailService
{
    void SendEmail(string email,string subject,string body);
}
