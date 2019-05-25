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
            user = this.Input.Text;
            parent.setUser(user);
            this.Close();
        }
    }
}
