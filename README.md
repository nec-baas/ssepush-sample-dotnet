sse-push-testclient : .Net SDK SSE Push送受信サンプルアプリ
=================================================================

.NET SDK SSE(ServerSentEvents) Push機能の、インスタレーション登録とPush送受信を確認するサンプルアプリです。

アプリケーション 事前設定
-----------------------
SampleApplication/SampleApplication.cs 内の以下変数に
BaaSのテナントID、アプリID、アプリキー、APIベースURLなどを設定してください。

### 設定対象
* EndpointUrl
* TenantId
* AppId
* AppKey

アプリの動作
------------

* アプリ起動時に、インスタレーション(端末情報)をサーバに登録し、コールバックを登録します。
* 接続ボタンを押すと、SSE Pushサーバと接続し、Push受信を待機します。
* 切断ボタンを押すと、SSE Pushサーバと切断します。
* Push送信ボタンを押すと、SSE Pushサーバにメッセージを送信します。
  受信待機中の場合はメッセージを受信します。
* ログがアプリ画面に表示されます。

