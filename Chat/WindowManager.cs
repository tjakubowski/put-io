using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Chat
{
    public static class WindowManager
    {
        public static Window MainWindow;

        public static void OpenPage(Page page)
        {
            MainWindow.Content = page;
        }
    }
}
