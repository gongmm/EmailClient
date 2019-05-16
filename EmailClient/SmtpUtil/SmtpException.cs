using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient
{
    class SmtpException
    {
        public class ConnectionFaildException : Exception
        {
            public ConnectionFaildException()
                : base()
            {

            }

            public ConnectionFaildException(string message)
                : base(message)
            {

            }
        }

        public class AuthFaildException : Exception
        {
            public AuthFaildException()
                : base()
            {

            }

            public AuthFaildException(string message)
                : base(message)
            {

            }
        }
    }
}
