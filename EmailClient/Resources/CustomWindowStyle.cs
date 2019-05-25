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
        private void maximumWindow(object sender, RoutedEventArgs e)
        {
            Window win = (Window)
                ((FrameworkElement)sender).TemplatedParent;
            if (win is MainWindow)
            {
                MainWindow main = (MainWindow)win;
                if (main.WindowState == WindowState.Normal)
                {
                    main.normaltop = main.Top;
                    main.normalleft = main.Left;
                    main.normalwidth = main.Width;
                    main.normalheight = main.Height;

                    double top = SystemParameters.WorkArea.Top;
                    double left = SystemParameters.WorkArea.Left;
                    double right = SystemParameters.PrimaryScreenWidth - SystemParameters.WorkArea.Right;
                    double bottom = SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Bottom;
                    //gd_main.Margin = new Thickness(left, top, right, bottom);
                    main.Top = top;
                    main.Left = left;
                    main.Width = SystemParameters.WorkArea.Width;
                    main.Height = SystemParameters.WorkArea.Height;
                    main.WindowState = WindowState.Maximized;
                    main.setScroller(main.Height-130);
                }
                else
                {
                    main.WindowState = WindowState.Normal;

                    //必须先设置为0,在重新设值,若前后值一样,会失效 --拖动任务栏后,还原-始终显示在屏幕最左上方
                    main.Top = 0;
                    main.Left = 0;
                    main.Width = 0;
                    main.Height = 0;

                    main.Top = main.normaltop;
                    main.Left = main.normalleft;
                    main.Width = main.normalwidth;
                    main.Height = main.normalheight;
                    main.setScroller(330);
                    // gd_main.Margin = new Thickness(0);
                }

            }
            else
                return;
        }
        

    }
}
