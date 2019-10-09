using AuthenServices.Model;
using MailingServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MailingServices.Service
{
    public class EmailSender
    {
        public AuthMessageSenderOptions Options { get; }

        public EmailSender(AuthMessageSenderOptions senderOptions)
        {
            Options = senderOptions;
        }

        public static Task SendMailAsync(AccountDTO cust, string SUBJECT, string message)
        {
            string myMail = "anhnhse62597@fpt.edu.vn";
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            mail.From = new MailAddress(myMail);
            mail.To.Add(new MailAddress(cust.Email));
            mail.Subject = SUBJECT;
            mail.IsBodyHtml = true;
            mail.Body = message;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(myMail, "Darkknightst123");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            return smtp.SendMailAsync(mail);
        }

        public Task SendEmailAsync(string toEmail, string subject, string message)
        {
            MailMessage mail = GetMailMessage(toEmail, subject, message,
                Options.DefaultSenderEmail, Options.DefaultSenderDisplayName, Options.UseHtml);
            SmtpClient client = GetSmtpClient(Options.Domain, Options.Port, Options.RequiresAuthentication,
                Options.UserName, Options.Key, Options.UseSsl);

            return client.SendMailAsync(mail);
        }

        public void SendEmail(string toEmail, string subject, string message)
        {
            MailMessage mail = GetMailMessage(toEmail, subject, message,
                Options.DefaultSenderEmail, Options.DefaultSenderDisplayName, Options.UseHtml);
            SmtpClient client = GetSmtpClient(Options.Domain, Options.Port, Options.RequiresAuthentication,
                Options.UserName, Options.Key, Options.UseSsl);

            client.Send(mail);
        }

        private static MailMessage GetMailMessage(string toEmail, string subject, string message,
        string defaultSenderEmail, string defaultSenderDisplayName = null, bool useHtml = true)
        {
            MailAddress sender;

            if (string.IsNullOrEmpty(defaultSenderEmail))
            {
                throw new ArgumentException("No sender mail address was provided");
            }
            else
            {
                sender = !string.IsNullOrEmpty(defaultSenderDisplayName) ?
                    new MailAddress(defaultSenderEmail, defaultSenderDisplayName) : new MailAddress(defaultSenderEmail);
            }

            MailMessage mail = new MailMessage()
            {
                From = sender,
                Subject = subject,
                Body = message,
                IsBodyHtml = useHtml
            };
            mail.To.Add(toEmail);
            return mail;
        }

        private static SmtpClient GetSmtpClient(string host, int port, bool requiresAuthentication = true,
            string userName = null, string userKey = null, bool useSsl = false)
        {
            SmtpClient client = new SmtpClient();

            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentException("No domain was provided");
            }

            client.Host = host;

            if (port > -1)
            {
                client.Port = port;
            }

            client.UseDefaultCredentials = !requiresAuthentication;

            if (requiresAuthentication)
            {
                if (string.IsNullOrEmpty(userName))
                {
                    throw new ArgumentException("No user name was provided");
                }

                client.Credentials = new NetworkCredential(userName, userKey);
            }

            client.EnableSsl = useSsl;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            return client;
        }
    }
}
