using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient
{
    class SmtpNoSslClient : SmtpBaseClient
    {

        public SmtpNoSslClient(string host, int port)
                : base(host, port)
        {
            writer = new StreamWriter(netStream);
        }

        public SmtpNoSslClient(string host, int port, string email, string password, AuthType type)
            : base(host, port, email, password)
        {
            writer = new StreamWriter(netStream);

            Auth(email, password, type);
        }

        public SmtpNoSslClient(string serverName, int port, MailAddress mailFrom, string password, AuthType type)
            : base(serverName, port, mailFrom, password)
        {
            writer = new StreamWriter(netStream);

            Auth(mailFrom.Address, password, type);
        }

        protected override bool AuthLogin(string email, string password)
        {
            if (!CheckResponse(220).Res)
                return false;

            SendData($"ehlo {Environment.MachineName}");
            if (!CheckResponse(250).Res)
                return false;

            SendData($"auth login");
            if (!CheckResponse(334).Res)
                return false;

            SendData(Convert.ToBase64String(Encoding.Default.GetBytes($"{email}")));
            if (!CheckResponse(334).Res)
                return false;

            SendData(Convert.ToBase64String(Encoding.Default.GetBytes($"{password}")));
            if (!CheckResponse(235).Res)
                return false;
            return true;
        }

        protected override bool AuthPlain(string email, string password)
        {
            var encodedEmail = Convert.ToBase64String(Encoding.Default.GetBytes("\0" + email));
            var encodedPass = Convert.ToBase64String(Encoding.Default.GetBytes("\0" + password));

            if (!CheckResponse(220).Res)
                return false;

            SendData($"ehlo {Environment.MachineName}");
            if (!CheckResponse(250).Res)
                return false;

            SendData("auth plain " + encodedEmail + encodedPass);
            if (!CheckResponse(235).Res)
            {
                encodedEmail = Convert.ToBase64String(Encoding.Default.GetBytes("\0" + email.Split('@')[0]));
                SendData("auth plain " + encodedEmail + encodedPass);
                if (!CheckResponse(235).Res)
                    return false;
            }
            return true;
        }
    }
    
}
