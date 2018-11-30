using Nec.Nebula;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleApplication
{
    public partial class SampleApplication : Form
    {
        // Nebulaサーバ テナント情報
        public const string NebulaEndpointUrl = "";
        public const string TenantId = "";
        public const string AppId = "";
        public const string AppKey = "";
        
        // Pushクライアント
        NbSsePushReceiveClient client;

        delegate void SetTextCallback(string text);

        public SampleApplication()
        {
            InitializeComponent();
        }

        // アプリケーション実行時の処理
        private async void SampleApplication_Load(object sender, EventArgs e)
        {
            // Nebula 初期化
            InitNebula();

            // インスタレーションを登録する
            await RegisterInstallation();

            // コールバックを登録する
            RegisterCallback();
        }

        // Nebula 初期化
        private void InitNebula()
        {
            // Nebula の生成
            var service = NbService.GetInstance();

            // テナントID
            service.TenantId = TenantId;

            // アプリケーションID
            service.AppId = AppId;

            // アプリケーションキー
            service.AppKey = AppKey;

            // エンドポイントURI
            service.EndpointUrl = NebulaEndpointUrl;
        }

        // インスタレーションを登録する
        private async Task<NbSsePushInstallation> RegisterInstallation()
        {
            // インスタレーションを取得する
            var installation = NbSsePushInstallation.GetCurrentInstallation();

            // 必須プロパティを設定する
            installation.Channels = new HashSet<string> { "NewsChannel", "AlarmChannel" };
            installation.AllowedSenders = new HashSet<string> { "g:anonymous" };

            // オプションを設定する
            NbJsonObject options = new NbJsonObject();
            options.Add("email", "SseSampleApp@pushtest.com");
            installation.Options = options;

            // インスタレーションをサーバに登録する
            return await installation.Save();
        }

        // コールバックを登録する
        private void RegisterCallback()
        {
            // NbSsePushReceiveClient を生成
            client = new NbSsePushReceiveClient();

            // メッセージ受信コールバック
            client.RegisterOnMessage("message", msg =>
            {
                // msg.Data に受信メッセージ(文字列)が引き渡される
                SetText("Message: " + msg.Data);
            });

            // エラーコールバック
            client.RegisterOnError((statusCode, errorInfo) =>
            {
                // エラー処理を行う
                SetText("Error: " + statusCode.ToString() + ", " + errorInfo.ToString());
            });

            // 接続完了時コールバック
            client.RegisterOnOpen(() =>
            {
                SetText("Open");
            });

            // 切断時コールバック
            client.RegisterOnClose(() =>
            {
                SetText("Close");
            });
        }

        // 接続ボタン押下時
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            client.Connect();
        }

        // 切断ボタン押下時
        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            client.Disconnect();
        }

        // Push送信ボタン押下時
        private async void SendMessageButton_Click(object sender, EventArgs e)
        {
            // Push送信
            await SendPush();
        }

        // Push送信
        private async Task<NbJsonObject> SendPush()
        {
            var push = new NbPush();

            // 宛先を作成する
            var query = new NbQuery();
            query.EqualTo("_channels", "AlarmChannel");
            query.EqualTo("email", "SseSampleApp@pushtest.com");

            // 宛先をセットする
            push.Query = query;

            // メッセージをセットする
            push.Message = "Low battery.";

            // Pushを送信する
            return await push.SendAsync();
        }

        // ログクリアボタン押下時
        private void ClearLogButton_Click(object sender, EventArgs e)
        {
            LogTextBox.Clear();
        }

        // ログ表示
        private void SetText(string text)
        {
            if (this.LogTextBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.LogTextBox.AppendText(text + "\n");
            }
        }
    }
}
