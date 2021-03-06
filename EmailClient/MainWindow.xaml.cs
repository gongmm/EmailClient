﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EmailClient.Entity;
using EmailClient.windows;
using EmailClient;

/* Email one = new Email();
          one.Sender = "一个新的";
          one.Topic = "一个新的";
          List<Email> email1 = new List<Email>();
          email1.Add(one);
          receiveBox.ItemsSource = email1;*/

namespace EmailClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public double normaltop;
        public double normalleft;
        public double normalwidth;
        public double normalheight;
        private long thispos;
        private int port = 25;
        private List<Email> emailList=null;
        private String user = null;
        private String keyword = null;
        private String server = null;
        private bool hasLogin = false;
        private SmtpClient client = null;
        private String smtpServerName = null;
        private String popServerName = null;
        private int smtpPort = 25;
        private int popPort = 110;
        public void setLoginStatus(bool status)
        {
            this.hasLogin = status;
            if (status)
            {
                String userName = user.Substring(0,user.IndexOf("@"));
                this.UserLogin.Content = userName;
                this.SendButton.Visibility = Visibility.Visible;
                this.SendButton.IsEnabled = true;
                this.CleanButton.Visibility = Visibility.Hidden;
                this.CleanButton.IsEnabled = true;

                
                this.ContentText.IsReadOnly = false;
                this.TopicText.IsReadOnly = false;
                this.ReceiveText.IsReadOnly = false;
                refresh();
            }
            else {
                this.UserLogin.Content = "未登录";
            }
        }
        public void setPort(int port)
        {
            this.port = port;
        }
        public void setUser(String user) {
            this.user = user;
            String smtpServerName = "smtp." + user.Substring(user.IndexOf("@") + 1);
            String popServerName = "pop." + user.Substring(user.IndexOf("@") + 1);
            this.smtpServerName = smtpServerName;
            this.popServerName = popServerName;
        }
        public void setKeyword(String keyword)
        {
            this.keyword = keyword;
        }
        public void setServer(String server)
        {
            this.server = server;
        }
        public void setScroller(double height) {
            this.emails.Height = height;
        }
        public bool Authorize() {
            this.SenderText.Text = user;
            SmtpClient smtpClient = null;
            try
            {
                smtpClient = new SmtpClient(smtpServerName, smtpPort, true);
           
                smtpClient.Authorize(user, keyword);
            }
            catch (Exception ex) {
                this.noUser.Visibility = Visibility.Visible;
                this.hasUser.Visibility = Visibility.Hidden;
                String exs = ex.ToString();
                this.receiveBox.ItemsSource = null;
                return false;
            }
            this.client = smtpClient;

            this.noUser.Visibility = Visibility.Hidden;
            this.hasUser.Visibility = Visibility.Visible;
            return true;
            }
        private void previewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer viewer = scrollviewer;  //scrollview 为Scrollview的名字，在Xaml文件中定义。
            if (viewer == null)
                return;
            double num = Math.Abs((int)(e.Delta / 2));
            double offset = 0.0;
            if (e.Delta > 0)
            {
                offset = Math.Max((double)0.0, (double)(viewer.VerticalOffset - num));
            }
            else
            {
                offset = Math.Min(viewer.ScrollableHeight, viewer.VerticalOffset + num);
            }
            if (offset != viewer.VerticalOffset)
            {
                viewer.ScrollToVerticalOffset(offset);
                e.Handled = true;
            }
        }
        
             private void sendEmailWithoutSSL(object sender, RoutedEventArgs e)
        {
           /* String sendAddr = this.SenderText.Text;
            String receiveAddr = this.ReceiveText.Text;
            String topic = this.TopicText.Text;
            String content = this.ContentText.Text;*/
         
            
             //   SmtpClient smtpClient = new SmtpClient(server, port, true);
          /*  Attention error = null;
            try
            {
                smtpClient.Authorize(user, keyword);
            }
            catch (SmtpException.ConnectionFaildException ex) {
                error = new Attention("连 接 失 败！");
                error.ShowDialog();
                return;
            }
            catch (SmtpException.AuthFaildException ex) {
                error = new Attention("验 证 失 败！");
                error.ShowDialog();
                return;
            }*/

                MailAddress address = new MailAddress(this.ReceiveText.Text);
                MailMessage message = new MailMessage(address, this.TopicText.Text, this.ContentText.Text);
            Attention error = null;
            if (this.client == null) {
                error = new Attention("请 先 登 录！");
                error.ShowDialog();
                //  InputWindow input = new InputWindow(this);
                //input.ShowDialog();
                return;
            }
            bool success = false;
            for (int i=0;i<2;i++) {
                try
                {

                    this.client.Send(message);
                }
                catch (Exception ex) {
                    client = new SmtpClient(smtpServerName, smtpPort, true);
                    client.Authorize(user, keyword);
                    continue;
                }
                success = true;
                break;
            }
            if (success == false)
            {
                error = new Attention("发 送 失 败！");
                error.ShowDialog();
                return;
            }
            //   smtpClient.Dispose(); 
            Attention successReturn = new Attention("发 送 成 功！");
            successReturn.ShowDialog();
        }
            private void Login(object sender, RoutedEventArgs e)
        {
            InputWindow input = new InputWindow(this);
            if (hasLogin == true)
                input.Title = "切换账户";
            input.ShowDialog();
        }
        private void cleanEmail(object sender, RoutedEventArgs e)
        {

            bool isDeleted = EmailClient.Program.deleteOne(thispos + 1,this.user,this.keyword);
            if (isDeleted == true)
            {
                this.SenderText.Text = "";
                this.ReceiveText.Text = "";
                this.TopicText.Text = "";
                this.ContentText.Text = "";
                refresh();
            }

        }
        private void createNewEmail(object sender, RoutedEventArgs e)
        {
            if(user!=null)
                this.SenderText.Text = user;
            else
            this.SenderText.Text = "";
            this.ReceiveText.Text = "";
            this.TopicText.Text = "";
            this.ContentText.Text = "";
           this.SendButton.Visibility = Visibility.Visible;
            this.SendButton.IsEnabled = true;
            this.CleanButton.Visibility = Visibility.Hidden;
            this.CleanButton.IsEnabled = false;
            /*
            this.CleanButton.Visibility = Visibility.Visible;

            this.CleanButton.IsEnabled = true;*/

            this.CleanButton.IsEnabled = true;
            this.noUser.Visibility = Visibility.Hidden;
            this.hasUser.Visibility = Visibility.Visible;
            this.ContentText.IsReadOnly = false;
            this.TopicText.IsReadOnly = false;
            this.ReceiveText.IsReadOnly = false;
        }
        private void closeWindow(object sender, RoutedEventArgs e)
        {
            
        }
        private void changeSelection(object sender, RoutedEventArgs e)
        {
            Email email=(Email)this.receiveBox.SelectedItem;
            if (email == null)
                return;
            long pos = email.pos;
            thispos = pos;
            EmailClient.Program.connectPop(user, keyword);
            EmailClient.Program.EmailDetail detail = EmailClient.Program.GetDetail(pos);
            this.SenderText.Text = detail.From;
            this.SenderText.IsReadOnly = true;
            this.ContentText.Text = detail.Body;
            this.ContentText.IsReadOnly = true;
            this.TopicText.Text = detail.Subject;
            this.TopicText.IsReadOnly = true;
            this.ReceiveText.Text = detail.To;
            this.ReceiveText.IsReadOnly = true;
            this.SendButton.Visibility = Visibility.Hidden;
            this.SendButton.IsEnabled = false;
            this.CleanButton.Visibility = Visibility.Visible;
            this.CleanButton.IsEnabled = true;


        }


        public MainWindow()
        {
            InitializeComponent();
            if (user == null)
                this.UserLogin.Content = "未登录";
                      this.emails.Height = this.Height - 50 - 50-30;
                 /*     List<Email> emails = new List<Email>();
                       Email email = new Email();
                       email.Sender = "<2016302580101@whu.edu.cn>";
                       email.Content = "This is a test!";
                       int length = 30;
                       if (email.Content.Length < 30)
                           length = email.Content.Length;
                       email.ContentBrief = email.Content.Substring(0,length);
                       emails.Add(email);
                       email = new Email();
                       email.Sender = "<test@whu.edu.cn>";
                       email.Topic = "test2";
                       email.Content = "This is a test!";
                      length = 30;
                       if (email.Content.Length < 30)
                           length = email.Content.Length;
                       email.ContentBrief = email.Content.Substring(0, length);
                       emails.Add(email);
                       email = new Email();
                       email.Sender = "<able@whu.edu.cn>";
                       email.Topic = "test3";
                       email.Content = "This is a test!";
                       length = 30;
                       if (email.Content.Length < 30)
                           length = email.Content.Length;
                       email.ContentBrief = email.Content.Substring(0, length);
                       emails.Add(email);
                       email = new Email();
                       email.Sender = "<newone@whu.edu.cn>";
                       email.Topic = "test4";
                       email.Content = "This is a test!";
                       length = 30;
                       if (email.Content.Length < 30)
                           length = email.Content.Length;
                       email.ContentBrief = email.Content.Substring(0, length);
                       emails.Add(email);
                       email = new Email();
                       email.Sender = "<another@whu.edu.cn>";
                       email.Topic = "test";
                       email.Content = "This is a test!";
                       length = 30;
                       if (email.Content.Length < 30)
                           length = email.Content.Length;
                       email.ContentBrief = email.Content.Substring(0, length);
                       emails.Add(email);
                       email = new Email();
                       email.Sender = "<thisis@whu.edu.cn>";
                       email.Topic = "无主题";
                       email.Content = "This is a test!";
                       length = 30;
                       if (email.Content.Length < 30)
                           length = email.Content.Length;
                       email.ContentBrief = email.Content.Substring(0, length);
                       emails.Add(email);
                       email = new Email();
                       email.Sender = "<tryit@whu.edu.cn>";
                       email.Topic = "无主题";
                       email.Content = "This is a test!";
                       length = 30;
                       if (email.Content.Length < 30)
                           length = email.Content.Length;
                       email.ContentBrief = email.Content.Substring(0, length);
                       emails.Add(email);
                       email = new Email();
                       email.Sender = "<ok@whu.edu.cn>";
                       email.Topic = "无主题";
                       email.Content = "This is a test!";
                       length = 30;
                       if (email.Content.Length < 30)
                           length = email.Content.Length;
                       email.ContentBrief = email.Content.Substring(0, length);
                       emails.Add(email);*/
          List<Email> emails = getMailIntros();
           this.emailList = emails;
            this.receiveBox.ItemsSource = emails;
        }
        private List<Email> getMailIntros() {
            if (hasLogin == true)
            {
                
                    EmailClient.Program.connectPop(user, keyword);
                List<EmailClient.Program.SimpleIntro> list = EmailClient.Program.listIntros();
                    List<Email> emails = new List<Email>();
                    for (int i = 0; i < list.Count; i++)
                    {
                    Email email = new Email();
                        email.pos = list[i].pos;
                    if (list[i].title.Length > 15)
                    {
                        email.Sender = list[i].title.Substring(0, 15);
                    }
                    else email.Sender = list[i].title;
                   // email.ContentBrief = list[i].brief;
                    email.Topic = list[i].subject;
                    emails.Add(email);
                    }
                    return emails;
              
            }
            else {
                this.UserLogin.Content = "未登录";
                return null;
            }
        }
        private void refreshMailIntros(object sender, RoutedEventArgs e) {
            refresh();
        }


        private void refresh() {
            List<Email> emails = getMailIntros();
            this.emailList = emails;
            this.receiveBox.ItemsSource = emails;
        }
 }
}
