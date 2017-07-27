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
using System.Diagnostics;

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
            Global.InitTimer(this.Dispatcher, OnPulse);
            WinLogin wl = new WinLogin();
            wl.ShowDialog();
        }
        /// <summary>
        /// 100毫秒一次,可以用来修改界面
        /// </summary>
        void OnPulse()
        {
            if (string.IsNullOrWhiteSpace(Global.SessionId))
            {
                return;
            }
            var events = Global.Network?.get_events(Global.SessionId);
            for (int i = 0; i < events?.Count; i++)
            {
                var evt = events[i];
                DoMoves(evt.Moves);
                DoMessage(evt.Messages);
            }
            events?.Clear();
        }

        private void DoMessage(List<string> messages)
        {
            if (messages == null && messages.Count == 0)
            {
                return;
            }
            this.Title = messages[0];
        }

        private void DoMoves(List<event_move> moves)
        {

        }

        private void button_enter_Click(object sender, RoutedEventArgs e)
        {
            var r = Global.Network.join_game(Global.SessionId, Global.CurrentPlayerBaseInfo.Player_name);
            button_enter.IsEnabled = !r;
            this.Title = "";
        }

        private void button_get_players_Click(object sender, RoutedEventArgs e)
        {
            var players = Global.Network.get_player_list(Global.SessionId);
            Debug.Assert(players.Count > 0);
            Global.CurrentPlayerBaseInfo = players[0];
            button_get_players.IsEnabled = players.Count < 1;
        }
    }
}
