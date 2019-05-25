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
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Attention : Window
    {
        private void closeWindow(object sender, RoutedEventArgs e) {
            this.Close();
        }
        public Attention(String content)
        {
            InitializeComponent();
            //this.WindowContent.IsReadOnly = false;
            this.WindowContent.Text = content;
        }

    }
}
