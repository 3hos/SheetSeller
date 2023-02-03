using SheetSeller.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using SheetSeller.Models.Domain;
using SheetSeller.Models.DTO;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace SheetSeller.Repositories.Implement
{
    public class EmailService : IEmailService
    {
        private readonly UserManager<ApplicationUser> userManager;
        public EmailService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        private async Task<Status> SendEmailAsync(string email, string subject, string msg)
        {
            try
            {
                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("sheetsellermail@gmail.com", "spkpththposxwkur");
                    MailMessage message = new MailMessage();
                    message.To.Add(email);
                    message.From = new MailAddress("sheetsellermail@gmail.com");
                    message.Subject = subject;
                    message.Body = msg;
                    client.Send(message);
                    return new Status() { Message = "", StatusCode = 1 };
                }
            }
            catch (Exception ex)
            {
                return new Status() { Message = "Oops. Can not send e mail", StatusCode = 0 };
            }
        }
        public async Task<Status> SendConfirmationLink(string mail,string link)
        {
            var res = await SendEmailAsync(mail, "Email Confirmation", link);
            return res;
        }
    }
}