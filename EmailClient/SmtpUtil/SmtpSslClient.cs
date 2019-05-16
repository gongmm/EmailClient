using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient
{
    class SmtpSslClient : SmtpBaseClient
    {
        private string data;

        public SmtpSslClient(string host, int port)
            : base(host, port)
        {
            CreateSslStream();
        }

        public SmtpSslClient(string host, int port, string email, string password, AuthType type)
            : base(host, port, email, password)
        {
            CreateSslStream();
            Auth(email, password, type);
        }

        public SmtpSslClient(string serverName, int port, MailAddress mailFrom, string password, AuthType type)
            : base(serverName, port, mailFrom, password)
        {
            CreateSslStream();
            Auth(mailFrom.Address, password, type);
        }

        protected override bool AuthLogin(string email, string password)
        {
            if (!CheckResponse(220).Res)
                return false;

            SendData($"ehlo {Environment.MachineName}");
            var res = CheckResponse(250);
            data = res.Data;
            if (!res.Res)
                return false;

            CheckForSsl(data);

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
            var res = CheckResponse(250);
            data = res.Data;
            if (!res.Res)
                return false;

            CheckForSsl(data);

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

        private void CheckForSsl(string data)
        {
            if (data.Contains("STARTTLS"))
            {
                SendData($"starttls");
                if (CheckResponse(220).Res)
                    CreateSslStream();
            }
        }
    }
}
