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
            public void SendWithoutSSL()
            {
                // 初始化
                smtpClient = new SmtpClient("smtp.163.com", 25, false);
                //smtpClient.AuthType = AuthType.AuthPlain;
                try
                {
                    smtpClient.Authorize("gnaizgnaw@163.com", "610319MM");
                }
                catch(Exception e)
                {
                    if (e is SmtpException.ConnectionFaildException)
                        Console.WriteLine(e.ToString());
                    if (e is SmtpException.AuthFaildException)
                        Console.WriteLine(e.ToString());
                }
                
                MailAddress address = new MailAddress("784325366@qq.com");
                MailMessage message = new MailMessage(address, "smtpTest", "this is a test");

                try
                {
                    smtpClient.Send(message);

                }
                catch (Exception e)
                {
                    if (e is SmtpException.AuthFaildException)
                        Console.WriteLine(e.ToString());
                   
                }
                smtpClient.Dispose();

            }

            [TestMethod]
            public void SendWithSSL()
            {
                smtpClient = new SmtpClient("smtp.163.com", 25, true);
                //smtpClient.AuthType = AuthType.AuthPlain;
                smtpClient.Authorize("gnaizgnaw@163.com", "password");
                
                String mailTo = "784325366@qq.com";
                String targetName = "wangziang";
                String subject = "smtpTest";
                String body = "this is a test with ssl";

                smtpClient.Send(mailTo, targetName, subject, body);
                smtpClient.Dispose();

            }


        }
    }
}
