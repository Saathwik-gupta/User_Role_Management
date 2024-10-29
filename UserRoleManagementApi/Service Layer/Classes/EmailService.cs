using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpSettings = _configuration.GetSection("SmtpSettings");
        var smtpClient = new SmtpClient
        {
            Host = smtpSettings["Server"],
            Port = int.Parse(smtpSettings["Port"]),
            EnableSsl = bool.Parse(smtpSettings["EnableSSL"]),
            Credentials = new NetworkCredential(smtpSettings["SenderEmail"], smtpSettings["SenderPassword"])
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings["SenderEmail"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true // Set to true if your email has HTML content
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
