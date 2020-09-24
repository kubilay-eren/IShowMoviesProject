using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IShowMovies.Models
{
    public class Mail
    {
        public static void MailSender(string ToAdress, string Subject, string Body)
        {
            var fromAddress = new MailAddress("ishowmovie@gmail.com");//örnek mail adresi
            var toAddress = new MailAddress(ToAdress);

            using (var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "12345")
            })
            {
                using (var message = new MailMessage(fromAddress, toAddress) { Subject = Subject, Body = Body })
                {
                    smtp.Send(message);
                }
            }
        }
    }
}
