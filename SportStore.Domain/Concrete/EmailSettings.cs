using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SportStore.Domain.Concrete
{
   public class EmailSettings
    {
        public string MailToAdress { get; set; }= "orders@example.com";
        public string MailFromAdress { get; set; }= "sportsstore@example.com";
        public bool UseSsl { get; set; } = true;
        public string UserName { get; set; } = "MySmtpUsername";
        public string Password { get; set; } = "MySmtpUserPassword";
        public string ServerName { get; set; } = "smtp.example.com";
        public int ServerPort = 587;
        public bool writeAsFile = false;
        public string FileLocation { get; set; }= @"c:\sports_store_emails";

    }
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings _emailSettings;
        public EmailOrderProcessor(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }
        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpclient=new SmtpClient())
            {
                smtpclient.EnableSsl = _emailSettings.UseSsl;
                smtpclient.Host = _emailSettings.ServerName;
                smtpclient.Port = _emailSettings.ServerPort;
                smtpclient.UseDefaultCredentials = false;
                smtpclient.Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password);
                if(_emailSettings.writeAsFile)
                {
                    smtpclient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpclient.PickupDirectoryLocation = _emailSettings.FileLocation;
                    smtpclient.EnableSsl = false;
                }
                StringBuilder body = new StringBuilder();
                body.AppendLine("A new order has been submitted");
                body.AppendLine("---");
                body.AppendLine("Items:");
                foreach (var line in cart.Lines)
                {
                    var subTotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subTotal: {2:c}", line.Quantity, line.Product.Name, subTotal);
                }
                body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue());
                body.AppendLine("---");
                body.AppendLine("Ship to:");
                body.AppendLine(shippingDetails.Name);
                body.AppendLine(shippingDetails.Line1 ?? "");
                body.AppendLine(shippingDetails.Line2 ?? "");
                body.AppendLine(shippingDetails.Line3 ?? "");
                body.AppendLine(shippingDetails.City);
                body.AppendLine(shippingDetails.State ?? "");
                body.AppendLine(shippingDetails.Country);
                body.AppendLine(shippingDetails.Zip);
                body.AppendLine("---");
                body.AppendFormat("Gift wrap: {0}", shippingDetails.GiftWrap ? "Yes":"No");
                MailMessage mailMessage = new MailMessage(_emailSettings.MailFromAdress,_emailSettings.MailToAdress,"new order submitted",body.ToString());
                if(_emailSettings.writeAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;

                }
                smtpclient.Send(mailMessage);
            }
        }
    }
}
