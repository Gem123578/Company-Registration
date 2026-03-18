using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Company_Registration_API.Utils
{
    public class EmailHelper
    {
        public static void SendConfirmationEmail(string email, string link)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("maythagyan.mtg.gl@gmail.com");
            message.To.Add(email);
            message.Subject = "Confirm your account";
            message.Body = $"Click this link to confirm your email:<br/><a href='{link}'>Confirm Email</a>";
            message.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("maythagyan.mtg.gl@gmail.com", "xsez zcpv spbq yfar");
            smtp.EnableSsl = true;

            smtp.Send(message);
        }
    }
}