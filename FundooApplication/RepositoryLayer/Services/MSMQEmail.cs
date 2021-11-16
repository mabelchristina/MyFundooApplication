using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace RepositoryLayer.Services
{
    public class MSMQEmail
    {
        public static void SendEmail(string token)
        {
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential("test.any.code.here@gmail.com", "test.any.code.here.182");

                MailMessage msgObj = new MailMessage();
                msgObj.To.Add("maybelchristina@gmail.com");
                msgObj.IsBodyHtml = true;
                msgObj.From = new MailAddress("test.any.code.here@gmail.com");
                msgObj.Subject = "Password Reset Link";
                msgObj.Body = "<html><body><p><b>Hello </b>,<br/>click the below link for reset password.<br/>" +
                    $"www.fundooapp.com/reset-password/{token}" +
                    "<br/><br/><br/><b>Warm Regards </b><br/><b>Mail Team(donot - reply to this mail)</b></p></body></html>";
                client.Send(msgObj);
            }
        }
    }
}
