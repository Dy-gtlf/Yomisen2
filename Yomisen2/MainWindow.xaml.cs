using Discord;
using Discord.WebSocket;
using FNF.Utility;
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

namespace Yomisen2
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDList iDList;
        private DiscordSocketClient client;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // トークンが存在しないならトークンの取得を行う
            if (string.IsNullOrEmpty(Properties.Settings.Default.Token))
            {
                var loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
            }

            client = new DiscordSocketClient();
            await client.LoginAsync(TokenType.User, Properties.Settings.Default.Token);
            await client.StartAsync();
            MessageBox.Show("ログインしました。", "ログイン成功");

            // メッセージ受信時のイベントを追加
            client.MessageReceived += Talk;

            // IDリストの読み込み
            iDList = XMLHelper.Deserialize<IDList>(@"IDList.xml");
            if (iDList == null)
            {
                MessageBox.Show("IDList.xmlが見つかりませんでした。\n新規作成します。");
                XMLHelper.Serialize("IDList.xml", new IDList());
                iDList = XMLHelper.Deserialize<IDList>(@"IDList.xml");
            }
            ChannelIDListGrid.ItemsSource = iDList.ChannelIDList;
            UserIDListGrid.ItemsSource = iDList.UserIDList;

            // DataGridの整形
            ChannelIDListGrid.Columns[0].Width = 150;
            ChannelIDListGrid.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            UserIDListGrid.Columns[0].Width = 150;
            UserIDListGrid.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Star);

        }

        /// <summary>
        /// メッセージを受け取った時の処理
        /// </summary>
        private async Task Talk(SocketMessage message)
        {
            await Task.Run(() =>
            {
                // 読み上げ
                using (var bc = new BouyomiChanClient())
                {
                    bc.AddTalkTask(message.Content);
                }
            });
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // 終了
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("ログアウトしますか？",
                "ログアウトの確認",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // トークンのリセット
                Properties.Settings.Default.Token = null;
                Properties.Settings.Default.Save();
                MessageBox.Show("ログアウトしました。");
                // トークンの取得を行う
                var loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            XMLHelper.Serialize("IDList.xml", iDList);
        }
    }
}
