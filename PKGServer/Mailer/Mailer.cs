using AnglicanGeek.MarkdownMailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace PKGServer.Mailer
{
    public class Mailer
    {
        private static MailSenderConfiguration Config = new MailSenderConfiguration() {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Host = "smtp.gmail.com",
            Port = 587,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("weappin@gmail.com", "s0lut10ns w34pp1n"),
            EnableSsl = true
        };

        public static void SendMail(string to, string subject, string body)
        {
            var mailer = new MailSender(Config);
            mailer.Send("weappin@gmail.com", to, subject, body);
        }
    }
}