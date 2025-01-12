using DevOpsExmaProject.Identity.Services.Abstracts;
using System.Net.Mail;
using System.Net;

namespace DevOpsExmaProject.Identity.Services.Concretes
{
    public class SMTPService : ISMTPService
    {
        private readonly IConfiguration _configuration;

        public SMTPService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendSMTP(string recipientEmail, int randomCheckCode)
        {
            string senderEmail = _configuration["SMTP:Email"];
            string senderPassword = _configuration["SMTP:Code"];


            string emailBody = $"<div> <h1>Email Verification Code Mp3</h1> <h2>{randomCheckCode}</h2> </div>";
   

            MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail)
            {
                Subject = "Email Verification Code",
                Body = emailBody,
                IsBodyHtml = true
            };

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = int.Parse(_configuration["SMTP:PortNum"]),
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true
            };

            smtpClient.Send(mailMessage);
        }
    }
}
