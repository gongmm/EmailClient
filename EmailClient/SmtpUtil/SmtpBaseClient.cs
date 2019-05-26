using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EmailClient
{
    internal class Result
    {
        public bool Res { get; set; }

        public string Data { get; set; }

        public Result(bool res, string data)
        {
            Res = res;
            Data = data;
        }
    }

    internal abstract class SmtpBaseClient
    {
        protected Socket socket;
        protected NetworkStream netStream;
        protected StreamWriter writer;
        protected SslStream sslStream;

        private IPHostEntry host;
        private IPEndPoint endPoint;
        private string serverName;

        private const string fileBoundary = "KkK170891tpbkKk__FV_KKKkkkjjwq";

        internal MailAddress MailFrom
        { get; set; }

        internal string Password
        { private get; set; }

        internal string ServerName
        {
            get
            {
                return serverName;
            }
            set
            {
                serverName = value;
                host = Dns.GetHostEntry(value);
            }
        }

        internal int Port
        { get; set; }

        internal bool IsAuthorized
        { get; set; }

        internal string BodyType
        { get; set; } = "text/plain";

        protected bool enableSsl;

        protected abstract bool AuthLogin(string email, string password);

        protected abstract bool AuthPlain(string email, string password);

        internal SmtpBaseClient(string serverName, int port)
        {
            ServerName = serverName ?? throw new ArgumentNullException();
            Port = port;
            Start(serverName, port);
        }

        internal SmtpBaseClient(string serverName, int port, string email, string password)
            : this(serverName, port)
        {
            if (email == null && password == null)
                throw new ArgumentNullException();
            MailFrom = new MailAddress(email);
            Password = password;
        }

        internal SmtpBaseClient(string serverName, int port, MailAddress mailFrom, string password)
            : this(serverName, port)
        {
            if (mailFrom == null && password == null)
                throw new ArgumentNullException();
            MailFrom = mailFrom;
            Password = password;
        }

        protected void Start(string serverName, int port)
        {
            endPoint = new IPEndPoint(host.AddressList[0], port);
            socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult result = socket.BeginConnect(endPoint, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(100, true);
            socket.ReceiveTimeout = 10000;
            if (!success)
                throw new SmtpException.ConnectionFaildException("Timeout expired. Unable to connect.");
            netStream = new NetworkStream(socket);
        }

        protected void SendData(string command)
        {
            if (command != null)
            {
                writer.WriteLine(command);
                writer.Flush();
            }
        }

        protected Result CheckResponse(int code)
        {
            byte[] buffer = new byte[512];
            if (!enableSsl)
                netStream.Read(buffer, 0, buffer.Length);
            else
                sslStream.Read(buffer, 0, buffer.Length);

            string line = Encoding.Default.GetString(buffer);
            line = line.Remove(line.IndexOf("\0"));

            if (line.Substring(0, 3) == code.ToString())
                return new Result(true, line);
            if (line.Substring(0, 3) == 535.ToString())
                return new Result(false, line);
            return new Result(false, "");
        }

        protected void CreateSslStream()
        {
            try
            {
                sslStream = new SslStream(netStream, false,
                    new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                sslStream.AuthenticateAsClient(ServerName);
                writer = new StreamWriter(sslStream);
                enableSsl = true;
            }
            catch (IOException)
            {
                socket.Close();
                Start(ServerName, Port);
                writer = new StreamWriter(netStream);
                enableSsl = false;
            }
        }

        internal void Auth(string email, string password, AuthType type)
        {
            try
            {
                MailFrom = new MailAddress(email);
                switch (type)
                {
                    case AuthType.AuthLogin:
                        IsAuthorized = AuthLogin(email, password);
                        break;
                    case AuthType.AuthPlain:
                        IsAuthorized = AuthPlain(email, password);
                        break;
                }
                if (!IsAuthorized)
                    throw new SmtpException.AuthFaildException("Email or password are incorrect");
            }
            catch (Exception e)
            {
                if (e is IOException)
                    throw new SmtpException.ConnectionFaildException("Timeout expired. Unable to connect.");
                throw;
            }
        }

        internal void Send(string targetMail, string targetName, string subject, string body)
        {
            if (!IsAuthorized)
                throw new SmtpException.AuthFaildException("Authorize first");

            SendData($"mail from: <{MailFrom.Address}>");
            if (!CheckResponse(250).Res)
                return;

            SendData($"rcpt to:<{targetMail}>");
            if (!CheckResponse(250).Res)
                return;

            SendData($"DATA");
            if (!CheckResponse(354).Res)
                return;

            //SendData($@"From: {MailFrom.DisplayName} {MailFrom.Address}
            //            To: {targetName} {targetMail}
            //            Subject: {subject}
            //            MIME-Version: 1.0
            //            Content-Type: text/plain; boundary={fileBoundary}
            //            --{fileBoundary}
            //            Content-Type: text/plain 
            //            {body}
            //            --{fileBoundary}--
            //          .");
            SendData($@"From: {MailFrom.DisplayName} {MailFrom.Address}
To: {targetName} {targetMail}
Subject: {subject}

{body}
.");
            if (!CheckResponse(250).Res)
                return;
        }

        internal void Dispose()
        {
            SendData("quit");
            socket = null;
            netStream = null;
            sslStream = null;
            GC.Collect();
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            return false;
        }


    }


}
