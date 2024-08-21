using DAL.Domain;
using DAL.Services;
using System.IO.Pipes;

namespace Server;

public partial class ServerPage : ContentPage {

    private readonly NamedPipeServerStream pipeServer;

    public ServerPage() {
        InitializeComponent();

        pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.InOut);
    }

    private async void SendMessage_Clicked(object sender, EventArgs e) {
        string message = MessageEntry.Text is null ? "null" : MessageEntry.Text;

        await pipeServer.WaitForConnectionAsync();
        StreamString ss = new(pipeServer);
        ss.WriteString(message);
        pipeServer.Close();
    }
}
