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
using Client;

namespace testClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_login_Click(object sender, RoutedEventArgs e)
        {
            Global.STT = new SingleThreadTimer(this.Dispatcher, OnPulse);
            WinLogin wl = new WinLogin();
            wl.ShowDialog();
        }
        /// <summary>
        /// 100毫秒一次,可以用来修改界面
        /// </summary>
        void OnPulse()
        {
            var events = Global.Network.get_events();
            for (int i = 0; i < events.Count; i++)
            {
                var evt = events[i];
                DoMoves(evt.Moves);
            }
            events.Clear();
        }

        private void DoMoves(List<event_move> moves)
        {

        }

        private void button_enter_Click(object sender, RoutedEventArgs e)
        {
            var r = Global.Network.join_map("map1");
            button_enter.IsEnabled = !r;
        }
    }
}
