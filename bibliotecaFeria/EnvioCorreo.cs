using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace bibliotecaFeria
{
    public class EnvioCorreo
    {
        public string correoTo { get; set; }


        public string enviarCorreoTransportista(string to, string idventa1, string html1)
        {

            System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
            correo.From = new System.Net.Mail.MailAddress("feriavirtualmg1@gmail.com", "Maipo Grande", System.Text.Encoding.UTF8);//Correo de salida
            correo.To.Add(to); //Correo destino?
            correo.Subject = "Contrato Registrado a Transportista en Feria Virtual"; //Asunto

            correo.Body = "Codigo de la Venta : " + idventa1 + " " + html1 + "Gracias por confiar en nosotros !"; //Mensaje del correo
            correo.IsBodyHtml = true;

            correo.Priority = MailPriority.Normal;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.gmail.com"; //Host del servidor de correo
            smtp.Port = 25; //Puerto de salida
            smtp.Credentials = new System.Net.NetworkCredential("feriavirtualmg1@gmail.com", "hantmgmsgkvsljkm");//Cuenta de correo
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            smtp.EnableSsl = true;//True si el servidor de correo permite ssl

            try
            {
                smtp.Send(correo);
                return "Correo enviado exitosamente";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        public string enviarCorreoCliente(string to, string html)
        {

            System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
            correo.From = new System.Net.Mail.MailAddress("feriavirtualmg1@gmail.com", "Maipo Grande", System.Text.Encoding.UTF8);//Correo de salida
            correo.To.Add(to); //Correo destino?
            correo.Subject = "Boleta Cliente Feria Virtual"; //Asunto
            correo.Body = html + " " + " Favor de Ingresa a la pagina web para porceder el pago con el Codigo de venta."; //Mensaje del correo
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.Normal;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.gmail.com"; //Host del servidor de correo
            smtp.Port = 25; //Puerto de salida
            smtp.Credentials = new System.Net.NetworkCredential("feriavirtualmg1@gmail.com", "hantmgmsgkvsljkm");//Cuenta de correo
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            smtp.EnableSsl = true;//True si el servidor de correo permite ssl

            try
            {
                smtp.Send(correo);
                return "Correo enviado exitosamente";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
    }
}
