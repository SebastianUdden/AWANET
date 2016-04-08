using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;


namespace AWANET.Models
{
    public static class MailSender
    {
        public static void SendTo(string address, string password, bool isReset)
        {
            //lägga in kontroll av formatet för att säkra E-post
            try
            {
                //Skapa själva epostmeddelandet med indata "from" och "to"
                MailMessage mail = new MailMessage("awacademynet@gmail.com", address);
                //skapa en E-postklient med server och port
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                //Eftersom min server kräver SSL aktiverar jag detta. Sätt till false om annan server utan SSL nyttjas
                client.EnableSsl = true;
                //Min server kräver login, lagrar mitt inlogg i en Credential.
                NetworkCredential myLogin = new NetworkCredential("awacademynet@gmail.com", "awa2016!");
                //Talar om hur smtpklienten ska kommunicera
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //Stänger av default cred. för att kunna lägga till mina egna inloggningsuppgifter.
                client.UseDefaultCredentials = false;
                //Lägger på myLogin, en credential där jag lagrat användarnamn och lösenord.
                client.Credentials = myLogin;
                //Lägger in txt i mejlets ämnesrad.
                mail.Subject = "User Credentials";
                //Väljer encoding för att kunna skicka ÅÄÖ
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                //skapa body i mailet.

                //Lägger in namn , adresser och telefonnummer i mailet.
                if (isReset)
                {
                    mail.Body = $"Lösenordet är återställt! Ditt användarnamn är {address}.\nDitt nya lösenord är {password}";
                }
                else
                {
                    mail.Body = $"Välkommen till AWAnet! Ditt användarnamn är {address}.\nDitt lösenord är {password}";
                }

                mail.Body += "\nFramtiden är här ☺ \n\n\t http://awanet.azurewebsites.net";
                //Skicka mail!
                client.SendAsync(mail, null);
            }
            catch (Exception ex)
            {
                //implementera loggfunktion för fel!
                //MessageBox.Show(ex.Message); 
            }
        }
    }
}
