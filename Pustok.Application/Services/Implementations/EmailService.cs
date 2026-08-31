using Pustok.Application.Services.Abstractions;
using System.Net;
using System.Net.Mail;

namespace Pustok.Application.Services.Implementations;

public class EmailService : IEmailService
{
    public void SendEmail(string email, string subject, string body)
    {
        NetworkCredential credential = new NetworkCredential("imedetzade5@gmail.com", "rubj kmyi hsew kdzd ");
        var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("imedetzade5@gmail.com");
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        mailMessage.To.Add(new MailAddress(email));
        mailMessage.IsBodyHtml = true;
        using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
        {
            smtpClient.Credentials = credential;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }
    }
}
