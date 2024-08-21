using DAL.Domain;
using DAL.Services;
using System.IO.Pipes;

namespace Client;

public partial class ClientPage : ContentPage {
    private readonly NamedPipeClientStream pipeClient;

    public ClientPage() {
        InitializeComponent();

        pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut, PipeOptions.None);

        ListenForMessages();
    }

    private async void ListenForMessages() {

        while(true) {
            if (!pipeClient.IsConnected) {
                await pipeClient.ConnectAsync();
            }
            
            StreamString ss = new(pipeClient);
            string message = await Task.Run(ss.ReadString());

            MainThread.BeginInvokeOnMainThread(() => {
                MessageLabel.Text = message;
            });
        }

    }
}
