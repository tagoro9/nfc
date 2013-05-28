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
            Host = "smtp.mailgun.org",
            Port = 587,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("postmaster@app13244.mailgun.org", "4h-hlrm5dei4"),
            EnableSsl = true
        };

        public static void SendMail(string to, string subject, string body)
        {
            var mailer = new MailSender(Config);
            mailer.Send("noreply@pkg.apphb.com", to, subject, body);
        }
    }
}