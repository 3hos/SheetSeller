using SheetSeller.Repositories.Abstract;
using SheetSeller.Models.DTO;
using System.Net;
using System.Net.Mail;

namespace SheetSeller.Repositories.Implement
{
    public class EmailService : IEmailService
    {
        private readonly string pas = Environment.GetEnvironmentVariable("SsEmailPassword", EnvironmentVariableTarget.Machine);
        private async Task<Status> SendEmailAsync(string email, string subject, string msg)
        {
            try
            {
                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("sheetsellermail@gmail.com", pas);
                    MailMessage message = new MailMessage();
                    message.To.Add(email);
                    message.From = new MailAddress("sheetsellermail@gmail.com");
                    message.Subject = subject;
                    message.Body = msg;
                    await client.SendMailAsync(message);
                    return new Status() {StatusCode = 1 };
                }
            }
            catch
            {
                return new Status() { Message = "Oops. Can not send e mail", StatusCode = 0 };
            }
        }
        public async Task<Status> SendConfirmationLink(string username, string mail,string link)
        {
            var res = await SendEmailAsync(mail, "Email Confirmation", $"Welcome {username}\r\n\r\nThanks for signing up with Sheet Seller!\r\nYou must follow this link to activate your account:\n{link}");
            return res;
        }
        public async Task<Status> SendRestoreLink(string username, string mail, string link)
        {
            var res = await SendEmailAsync(mail, "Restore password", $"Hello {username}\r\n\r\nYou must follow this link to reset your password:\n{link}\n If you have not tried to reset your email don't do anything");
            return res;
        }
    }
}