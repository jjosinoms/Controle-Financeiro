using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SistemaContas.Messages.Services
{
    public class EmailService
    {
        #region Conta de email para envios automáticos do sistema

        private static string _conta = "cotiaulajava@outlook.com";
        private static string _senha = "@Admin123456";
        private static string _smtp = "smtp-mail.outlook.com";
        private static int _porta = 587;

        #endregion

        public static void EnviarMensagem(string emailDest, string assunto, string mensagem)
        {
            #region Montando conteudo do email

            var mailMessage = new MailMessage(_conta, emailDest);
            mailMessage.Subject = assunto;
            mailMessage.Body = mensagem;
            mailMessage.IsBodyHtml = true;

            #endregion

            #region Enviando o email

            var smtpCliente = new SmtpClient(_smtp, _porta);
            smtpCliente.EnableSsl = true; // Security Socket Layer
            smtpCliente.Credentials = new NetworkCredential(_conta, _senha);
            smtpCliente.Send(mailMessage);

            #endregion
        }


    }
}
