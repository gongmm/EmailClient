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
using System.Windows.Shapes;

namespace EmailClient.windows
{
    /// <summary>
    /// InputWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputWindow : Window
    {
        private String user=null;
        MainWindow parent;
        public InputWindow(MainWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
        }
        private void closeWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void login(object sender, RoutedEventArgs e)
        {
            user = this.userName.Text;
            String keyword = this.keyword.Password;

            // String serverName = this.serverName.Text;
            String smtpServerName = "smtp."+user.Substring(this.user.IndexOf("@")+1);
            String pop3ServerName = "pop3." + user.Substring(this.user.IndexOf("@") + 1);
            parent.setUser(user);
            parent.setKeyword(keyword);
           // parent.setServer();
           // parent.setPort(Convert.ToInt32(this.portName.Text));
            bool result=parent.Authorize();
            if (result == false) {
                parent.setLoginStatus(false);
                Attention attention = new Attention("登 陆 失 败");
                attention.ShowDialog();
                return;
            }
            parent.setLoginStatus(true);
            this.Close();
        }
    }
}
