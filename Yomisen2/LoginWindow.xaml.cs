using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Yomisen2
{
    /// <summary>
    /// LoginWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LoginWindow : Window
    {
        private bool isWaitting = false;

        public LoginWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// トークンの取得
        /// </summary>
        /// <returns></returns>
        private async Task Login()
        {
            isWaitting = true;
            LoginButton.IsEnabled = false;
            // メールアドレスとパスワードでトークンを取得
            var email = Email.Text;
            var password = Password.Password;
            var token = await DiscordApiHelper.LogInAsync(email, password);
            if (!string.IsNullOrEmpty(token))
            {
                Properties.Settings.Default.Token = token;
                Properties.Settings.Default.Save();
                Close();
            }
            else
            {
                MessageBox.Show("ログインに失敗しました。", "ログイン失敗");
                isWaitting = false;
                LoginButton.IsEnabled = true;
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            await Login(); // トークンの取得
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.Token))
            {
                Environment.Exit(0); // アプリケーションの終了
            }
        }

        private async void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && !isWaitting) // エンターキー入力でもログイン可能
            {
                await Login(); // トークンの取得
            }
        }
    }
}
