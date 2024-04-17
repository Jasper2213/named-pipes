using DAL.Domain;
using DAL.Services;
using System.IO.Pipes;

namespace Client;

public partial class ClientPage : ContentPage {
    private readonly NamedPipeClientStream _pipeClient;
    private readonly StreamString ss;
    private bool _isConnected = false;

    public ClientPage() {
        InitializeComponent();

        _pipeClient = new NamedPipeClientStream(".", PipeService.PipeName, PipeDirection.In, PipeOptions.Asynchronous);
        ss = new StreamString(_pipeClient);

        InitPipeClient();
    }

    private async void InitPipeClient() {
        if (_isConnected) {
            return;
        }

        if (!_pipeClient.IsConnected) {
            await _pipeClient.ConnectAsync().ConfigureAwait(false);
        }

        _isConnected = true;

        ListenForMessages();
    }

    protected override void OnDisappearing() {
        base.OnDisappearing();

        _pipeClient.Close();
        _pipeClient.Dispose();

        _isConnected = false;
    }

    private async void ListenForMessages() {
        if (!_pipeClient.IsConnected) {
            return;
        }

        string message = await Task.Run(ss.ReadString);

        MainThread.BeginInvokeOnMainThread(() => {
            MessageLabel.Text = message;
        });

        ListenForMessages();
    }
}
