using System;
using System.Net.Mail;

namespace Company_Registration_API.Utils
{
    public class EmailHelper
    {
        public static void SendConfirmationEmail(string email, string confirmLink)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.To.Add(email);
                message.Subject = "Confirm Your Account";

                message.Body = $@"
                    <h3>Welcome!</h3>
                    <p>Please confirm your email:</p>
                    <a href='{confirmLink}' 
                       style='padding:10px 20px;background-color:#007bff;color:white;text-decoration:none;border-radius:5px;'>
                       Confirm Email
                    </a>
                    <br/><br/>
                    <p>If you did not request this, ignore this email.</p>
                ";

                message.IsBodyHtml = true;

                
                SmtpClient smtp = new SmtpClient();
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception("Email sending failed: " + ex.Message);
            }
        }
    }
}