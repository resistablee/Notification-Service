using Newtonsoft.Json;
using NotificationService.Models.Entities;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;

namespace NotificationService.Models
{
    public class NotificationMail
    {
        private readonly IConfiguration _configuration;

        public NotificationMail(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMail(Email model)
        {
            Debug.WriteLine(JsonConvert.SerializeObject(model));

            try
            {
                string username = _configuration.GetValue<string>("Mail:Email");
                string password = _configuration.GetValue<string>("Mail:Password");

                using (SmtpClient client = new SmtpClient())
                {
                    client.Port = 587;
                    client.Host = _configuration.GetValue<string>("Mail:HostAddress");
                    client.EnableSsl = true;
                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(username, password);
                    MailMessage mm = new MailMessage(username, model.Address, model.Subject, model.Message);
                    mm.IsBodyHtml = false;
                    mm.BodyEncoding = UTF8Encoding.UTF8;
                    mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    client.Send(mm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
