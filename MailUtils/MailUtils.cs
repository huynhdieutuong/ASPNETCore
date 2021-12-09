using System.Net.Mail;
using System.Threading.Tasks;

namespace MailUtils
{
    public class MailUtils
    {
        public static async Task<string> SendMail(string _from, string _to, string _subject, string _body)
        {
            // Setup email
            MailMessage message = new MailMessage(_from, _to, _subject, _body);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            message.ReplyToList.Add(new MailAddress(_from));
            message.Sender = new MailAddress(_from, "Name Sender");

            // Setup send mail
            using var smtpClient = new SmtpClient("localhost");
            try
            {
                await smtpClient.SendMailAsync(message);
                return "Sent mail successfully";
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
                return "Failure sending mail.";
            }
        }
    }
}