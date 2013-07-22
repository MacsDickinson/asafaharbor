using System;
using System.Configuration;
using System.Net.Mail;

namespace Asafaharbor.Web.Models
{
    public class Email
    {
        private readonly string _from;
        private readonly string _server;
        private readonly string _password;

        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public DateTime DateSent { get; set; }
        public bool SendSuccessful { get; set; }

        public Email()
        {
            _server = ConfigurationManager.AppSettings["Email_Server"];
            _password = ConfigurationManager.AppSettings["Email_Password"];
            _from = ConfigurationManager.AppSettings["Email_From"];
        }

        public static Email NewConfirmationEmail(string to, string confirmationLink, string name)
        {
            return new Email
            {
                Subject = "ASafaHarbor email confirmation",
                Body =
                    string.Format("<html>Hello {0},<br/><br/>Welcome to ASafaHarbor!<br /><br />Before we get started we need to confirm that this is your email. To do this follow thie link below:<br /><br /><a href=\"{1}\">{1}</a>", name, confirmationLink),
                To = to,
                DateSent = DateTime.Now,
                IsHtml = true
            };
        }

        public void Send()
        {
            try
            {
                SmtpClient smtpServer = new SmtpClient(_server);
                var mail = new MailMessage
                {
                    From = new MailAddress(_from)
                };
                foreach (string recipient in To.Split(';'))
                {
                    mail.To.Add(recipient);
                }

                mail.Subject = Subject;
                mail.IsBodyHtml = IsHtml;
                mail.Body = Body;
                smtpServer.Port = 587;
                smtpServer.UseDefaultCredentials = false;
                smtpServer.Credentials = new System.Net.NetworkCredential(_from, _password);
                smtpServer.EnableSsl = true;
                smtpServer.Send(mail);
                SendSuccessful = true;
            }
            catch (Exception)
            {
                SendSuccessful = false;
            }
        }
    }
}