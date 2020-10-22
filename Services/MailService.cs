using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Services
{
    public class MailService
    {
        public async Task SendConfirmationEmail(string address,string token)
        {
            var mailMessage = new MimeMessage();

            mailMessage.From.Add(new MailboxAddress("Администрация MyDeck", "mydeck.mailservice@gmail.com"));
            mailMessage.To.Add(new MailboxAddress("", address));
            mailMessage.Subject = "Email Confirmation";
            mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"<h1>Чтобы подтвердить свою почту перейдите по флагу Украины</h1> " +
                $"<h3>Если вы не совершали регистрацию просто проигнорируйте это сообщение</h3><div><a " +
                $"href='http://threetests-001-site1.gtempurl.com/mydeckapi/user/confirmemail/{token}'>" +
                $"<img src="+ "https://image.freepik.com/free-photo/flag-of-ukraine_1401-249.jpg \"></a></div>"
            };

            using(var client=new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 25, false);
                await client.AuthenticateAsync("mydeck.mailservice@gmail.com", "velfrvelfr1337");
                await client.SendAsync(mailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
