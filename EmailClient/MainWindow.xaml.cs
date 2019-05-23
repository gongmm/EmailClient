using System;
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
namespace EmailClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Email> emailList=null;
        
              private void createNewEmail(object sender, RoutedEventArgs e)
        {

            this.SenderText.Text = "";
            this.ReceiveText.Text = "";
            this.TopicText.Text = "";
            this.ContentText.Text = "";
           this.SendButton.Visibility = Visibility.Visible;
            this.SendButton.IsEnabled = true;
            this.CleanButton.Visibility = Visibility.Visible;
            this.CleanButton.IsEnabled = true;
        }
        private void closeWindow(object sender, RoutedEventArgs e)
        {
            
        }
        private void changeSelection(object sender, RoutedEventArgs e)
        {
            Email email=(Email)this.receiveBox.SelectedItem;
            if (email == null)
                return;
            this.SenderText.Text = email.Sender;
            this.ContentText.Text = email.Content;
            this.SendButton.Visibility = Visibility.Hidden;
            this.SendButton.IsEnabled = false;
            this.CleanButton.Visibility = Visibility.Hidden;
            this.CleanButton.IsEnabled = false;
        }
        public MainWindow()
        {
            InitializeComponent();
            this.emails.Height = this.Height - 50 - 50-30;
            List<Email> emails = new List<Email>();
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
            email.Content = "This is a test!";
           length = 30;
            if (email.Content.Length < 30)
                length = email.Content.Length;
            email.ContentBrief = email.Content.Substring(0, length);
            emails.Add(email);
            email = new Email();
            email.Sender = "<able@whu.edu.cn>";
            email.Content = "This is a test!";
            length = 30;
            if (email.Content.Length < 30)
                length = email.Content.Length;
            email.ContentBrief = email.Content.Substring(0, length);
            emails.Add(email);
            email = new Email();
            email.Sender = "<newone@whu.edu.cn>";
            email.Content = "This is a test!";
            length = 30;
            if (email.Content.Length < 30)
                length = email.Content.Length;
            email.ContentBrief = email.Content.Substring(0, length);
            emails.Add(email);
            email = new Email();
            email.Sender = "<another@whu.edu.cn>";
            email.Content = "This is a test!";
            length = 30;
            if (email.Content.Length < 30)
                length = email.Content.Length;
            email.ContentBrief = email.Content.Substring(0, length);
            emails.Add(email);
            email = new Email();
            email.Sender = "<thisis@whu.edu.cn>";
            email.Content = "This is a test!";
            length = 30;
            if (email.Content.Length < 30)
                length = email.Content.Length;
            email.ContentBrief = email.Content.Substring(0, length);
            emails.Add(email);
            email = new Email();
            email.Sender = "<tryit@whu.edu.cn>";
            email.Content = "This is a test!";
            length = 30;
            if (email.Content.Length < 30)
                length = email.Content.Length;
            email.ContentBrief = email.Content.Substring(0, length);
            emails.Add(email);
            email = new Email();
            email.Sender = "<ok@whu.edu.cn>";
            email.Content = "This is a test!";
            length = 30;
            if (email.Content.Length < 30)
                length = email.Content.Length;
            email.ContentBrief = email.Content.Substring(0, length);
            emails.Add(email);
            this.emailList = emails;
            this.receiveBox.ItemsSource = emails;
        }

    }
}
