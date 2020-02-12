using AuthenServices.Model;
using MailingServices.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace MailingServices.Service
{
    public class EmailSender
    {
        public AuthMessageSenderOptions Options { get; }

        public EmailSender(AuthMessageSenderOptions senderOptions)
        {
            Options = senderOptions;
        }

        public static Task SendMailAsync(string receiver, string SUBJECT, string message)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            mail.From = new MailAddress(AppConstrain.sender);
            mail.To.Add(new MailAddress(receiver));
            mail.Subject = SUBJECT;
            mail.IsBodyHtml = true;
            mail.Body = message;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(AppConstrain.sender, AppConstrain.senderPass);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            return smtp.SendMailAsync(mail);
        }

        public static void SendAccountMailAsync(MessageAccountDTO messageAccountDTO)
        {
            string subject = "Welcome To DEVERATE System";
            string FilePath = "MailTemplates/MailAccountTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string htmlBody = str.ReadToEnd();
            htmlBody = htmlBody.Replace("[fullname]", messageAccountDTO.Fullname);
            htmlBody = htmlBody.Replace("[username]", messageAccountDTO.Username);
            htmlBody = htmlBody.Replace("[password]", messageAccountDTO.Password);
            htmlBody = htmlBody.Replace("[passwordURL]", HttpUtility.UrlEncode(messageAccountDTO.Password));
            str.Close();
            SendMailAsync(messageAccountDTO.Email, subject, htmlBody);
        }

        public Task SendEmailAsync(string toEmail, string subject, string message)
        {
            MailMessage mail = GetMailMessage(toEmail, subject, message,
                Options.DefaultSenderEmail, Options.DefaultSenderDisplayName, Options.UseHtml);
            SmtpClient client = GetSmtpClient(Options.Domain, Options.Port, Options.RequiresAuthentication,
                Options.UserName, Options.Key, Options.UseSsl);

            return client.SendMailAsync(mail);
        }

        public static void SendTestEmployeeMailAsync(List<TestMailDTO> testMailDTOs)
        {
          foreach(TestMailDTO testMailDTO in testMailDTOs)
            {
                string subject = testMailDTO.title;
                string FilePath = "MailTemplates/MailEmployeeTestTemplate.html";
                StreamReader str = new StreamReader(FilePath);
                string htmlBody = str.ReadToEnd();
                htmlBody = htmlBody.Replace("[fullname]", testMailDTO.fullName);
                htmlBody = htmlBody.Replace("[title]", testMailDTO.title);
                htmlBody = htmlBody.Replace("[StartDate]", String.Format("{0:d/M/yyyy HH:mm:ss}", testMailDTO.startDate.AddHours(7)));
                htmlBody = htmlBody.Replace("[EndDate]", String.Format("{0:d/M/yyyy HH:mm:ss}", testMailDTO.endDate.AddHours(7)));
                htmlBody = htmlBody.Replace("[url]", "http://deverate-system.s3-website-ap-southeast-1.amazonaws.com/test/" + testMailDTO.testId);
                htmlBody = htmlBody.Replace("[code]", testMailDTO.code);
                str.Close();
                SendMailAsync(testMailDTO.email, subject, htmlBody);
            }
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
