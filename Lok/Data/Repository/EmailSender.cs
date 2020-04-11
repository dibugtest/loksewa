using Lok.Data.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Lok.Data.Repository
{
    public class EmailSender:IEmailSender
    {
        private IConfiguration configuration;

        public EmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(subject, htmlMessage, email);
        }

        public Task Execute(string subject, string message, string email)
        {
            SmtpClient client = new SmtpClient();
            client.Host = configuration.GetSection("Email").GetSection("Host").Value;
            client.Port = Convert.ToInt32(configuration.GetSection("Email").GetSection("SMTPPort").Value);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(configuration.GetSection("Email").GetSection("UserId").Value, configuration.GetSection("Email").GetSection("Password").Value);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("dbugtest2016@gmail.com");
            mailMessage.To.Add(email);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;





            return client.SendMailAsync(mailMessage);



        }


    }
}
