using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
namespace EmailClient.Resources
{
    public partial class CustomWindowStyle
    {
        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            Window win = (Window)
                ((FrameworkElement)sender).TemplatedParent;
            win.Close();
        }
        private void titleBar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Window win = (Window)
                ((FrameworkElement)sender).TemplatedParent;
            win.DragMove();
        }
        
    }
}
