using DAL.Domain;
using DAL.Services;
using System.IO.Pipes;

namespace Server;

public partial class ServerPage : ContentPage {
    private readonly NamedPipeServerStream _pipeServer;
    private readonly StreamString ss;

    public ServerPage() {
        InitializeComponent();

        _pipeServer = new NamedPipeServerStream(PipeService.PipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
        ss = new StreamString(_pipeServer);

        InitPipeServer();
    }

    private void InitPipeServer() {
        _pipeServer.BeginWaitForConnection(ar => {
            _pipeServer.EndWaitForConnection(ar);
        }, null);
    }

    protected override void OnDisappearing() {
        base.OnDisappearing();

        _pipeServer.Close();
        _pipeServer.Dispose();
    }

    private void SendMessage_Clicked(object sender, EventArgs e) {
        if (!_pipeServer.IsConnected) {
            return;
        }

        string message = MessageEntry.Text is null ? "null" : MessageEntry.Text;
        ss.WriteString(message);
    }
}
