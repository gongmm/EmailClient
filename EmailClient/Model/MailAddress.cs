using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient
{
    class MailAddress
    {
        public string Address { get; set; }

        public string DisplayName { get; set; }

        public MailAddress(string address)
        {
            Address = address ?? throw new ArgumentNullException();
        }

        public MailAddress(string address, string displayName)
            : this(address)
        {
            DisplayName = displayName;
        }

    }
}
