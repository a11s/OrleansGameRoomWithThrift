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

namespace testClient
{
    /// <summary>
    /// WinLogin.xaml 的交互逻辑
    /// </summary>
    public partial class WinLogin : Window
    {
        public WinLogin()
        {
            InitializeComponent();
        }

        private void button_connect_Click(object sender, RoutedEventArgs e)
        {
            Global.GetNetwork(textbox_ip.Text);
            button_connect.IsEnabled = Global.Network == null;

        }

        private void button_auth_Click(object sender, RoutedEventArgs e)
        {
            var r = Global.Network.auth(textbox_name.Text);
            Global.IsAuthSuccess = !string.IsNullOrWhiteSpace(r);
            if (Global.IsAuthSuccess)
            {
                Global.SessionId = r;
            }

            button_auth.IsEnabled = !Global.IsAuthSuccess;

        }
    }
}
