using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace EmailClient
{
    internal class Program
    {
        private static Pop3Client pop3Client;
        public struct SimpleIntro
        {
            public string title;
            public string subject;
            public long pos;
            public string sender;

        };

        public struct EmailDetail
        {
            public string From;
            public string To;
            public string Subject;
            public string Body;
        };
        
        public static List<SimpleIntro> listIntros()
        {
            List<SimpleIntro> list=new List<SimpleIntro>();
            long count = pop3Client.MessageCount;
           // Console.WriteLine(count);

            for (long i = 0; i <count-4; i++)
            {
                pop3Client.NextEmail();
                Console.WriteLine(pop3Client.From);
                
                SimpleIntro s=new SimpleIntro();
                s.title = pop3Client.From;
                s.subject = pop3Client.Subject;
                s.pos = i;
                s.sender = pop3Client.From;
                list.Add(s);
            }
            return list;
        }

        public static Pop3Client login(string user, string pass)
        {
            return new Pop3Client(user,pass,"pop.whu.edu.cn");
        }

        public static void connectPop(string user, string pass)
        {
            pop3Client=login(user,pass);
            pop3Client.OpenInbox();
        }

        public static EmailDetail GetDetail(long pos)
        {
            EmailDetail emailDetail=new EmailDetail();
            pop3Client.NextEmail(pos);
            emailDetail.From = pop3Client.From;
            emailDetail.To = pop3Client.To;
            emailDetail.Subject = pop3Client.Subject;
            emailDetail.Body = pop3Client.Body;
            return emailDetail;
        }

        public static bool deleteOne(long pos)
        {
            return pop3Client.DeleteEmail(pos);
        }
        
        
        
        
//        public static void Main(string[] args)
//        {
 //          connectPop("2016302580098@whu.edu.cn","ilearnai1973#");
   //         List<SimpleIntro>  list=new List<SimpleIntro>();
//            list = listIntros();
//            Console.WriteLine("\n");
//            Console.WriteLine(list[0].pos);
            
////            Console.WriteLine(GetDetail(2).From);
////            Console.WriteLine(deleteOne(0).ToString());



//        }

       
    }
}