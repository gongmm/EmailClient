using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient
{
    public enum AuthType
    {
        AuthLogin,
        AuthPlain
    }

    class SmtpClient
    {
        //private static String sender = "gnaizgnaw@163.com";
        //private static String receiver = "784325366@qq.com";
        //private static String SMTPServer = "163.com";
        private SmtpBaseClient client;
        private string host;
        private int? port;

    

        public bool EnableSsl { get; set; }

        public bool IsAuthorized { get; set; }

        public AuthType AuthType { get; set; } = AuthType.AuthLogin;

        public SmtpClient(string host, int port, bool enableSsl)
        {
            this.host = host;
            this.port = port;
            EnableSsl = enableSsl;
            if (enableSsl)
                client = new SmtpSslClient(host, port);
            else
                client = new SmtpNoSslClient(host, port);
        }

        public SmtpClient(string host, int port, string email, string password, bool enableSsl)
            : this(host, port, enableSsl)
        {
            if (enableSsl)
                client = new SmtpSslClient(host, port, email, password, AuthType);
            else
                client = new SmtpNoSslClient(host, port, email, password, AuthType);
            IsAuthorized = client.IsAuthorized;
        }

        public bool Authorize(string email, string password)
        {
            if (host == null || port == null)
                throw new ArgumentNullException();

            if (IsAuthorized)
                throw new SmtpException.AuthFaildException("You are still authorize");

            if (EnableSsl)
                client = new SmtpSslClient(host, port.Value, email, password, AuthType);
            else
                client = new SmtpNoSslClient(host, port.Value, email, password, AuthType);
            IsAuthorized = client.IsAuthorized;
            return IsAuthorized;
        }

        public void Send(string mailTo, string subject, string body)
        {
            Send(new MailMessage(mailTo, subject, body));
        }

        public void Send(string mailTo, string targetName, string subject, string body)
        {
            Send(new MailMessage(mailTo, targetName, subject, body));
        }

        public void Send(MailMessage message)
        {
            client.Send(message.MailTo.Address, message.MailTo.DisplayName,
                    message.Subject, message.Body);
        }

        public void Dispose()
        {
            client.Dispose();
            IsAuthorized = false;
            host = null;
            port = null;
        }

    }
}
