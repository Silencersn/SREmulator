using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace SREmulator.GUI.View
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : UserControl
    {

        public About()
        {
            InitializeComponent();
        }

        private void About_Loaded(object sender, RoutedEventArgs e)
        {
            // 获取应用程序版本号   
            txtVersion.Text = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // 使用默认浏览器打开链接
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        }
    }
}
