using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Entity
{
    class Email
    {
        public String Sender { get; set; }
        public String Receiver { get; set; }
        public String Topic { get; set; }
        public String Content{ get; set;}
        public String ContentBrief { get; set; }
    }
}
