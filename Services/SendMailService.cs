using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

public class SendMailService
{
    MailSettings _mailSettings { get; set; }
    public SendMailService(IOptions<MailSettings> mailSettings) // 5.7 Inject MailSettings into IOptions
    {
        _mailSettings = mailSettings.Value;
    }
    public async Task<string> SendMail(MailContent mailContent) // 5.8 Create SendMail
    {
        var email = new MimeMessage();
        email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
        email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));

        email.To.Add(new MailboxAddress(mailContent.To, mailContent.To));
        email.Subject = mailContent.Subject;

        var builder = new BodyBuilder();
        builder.HtmlBody = mailContent.Body;
        // Can add attachments: builder.Attachments
        email.Body = builder.ToMessageBody();

        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        try
        {

            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
        }
        catch (System.Exception e)
        {
            System.Console.WriteLine(e.Message);
            return "Failure send mail";
        }
        smtp.Disconnect(true);
        return "Sent email successfully";
    }
}

public class MailContent
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}