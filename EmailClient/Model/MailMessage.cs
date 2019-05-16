using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient
{
    class MailMessage
    {
        public MailAddress MailTo { get; set; }

        public string Subject { get; set; }

        public string Body
        { get; set; }

        public MailMessage(MailAddress mailTo, string subject, string body)
        {
            MailTo = mailTo ?? throw new ArgumentNullException();
            Subject = subject;
            Body = body;
        }

        public MailMessage(string mailTo, string subject, string body)
            : this(new MailAddress(mailTo), subject, body)
        {
        }

        public MailMessage(string mailTo, string targetName, string subject, string body)
            : this(new MailAddress(mailTo, targetName), subject, body)
        {
        }

    }
}

