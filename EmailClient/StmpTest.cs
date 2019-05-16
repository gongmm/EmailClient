using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmailClient
{
    class StmpTest
    {
        [TestClass]
        public class UnitTest1
        {
            private SmtpClient smtpClient;

            [TestMethod]
            public void Authorize()
            {
                smtpClient = new SmtpClient("smtp.163.com", 25, false);
                //smtpClient.AuthType = AuthType.AuthPlain;
                smtpClient.Authorize("gnaizgnaw@163.com", "610319MM");
                MailAddress address = new MailAddress("784325366@qq.com");
                MailMessage message = new MailMessage(address, "smtpTest", "this is a test");
                smtpClient.Send(message);
                smtpClient.Dispose();
                
            }
        }
    }
}
