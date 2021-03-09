using AsyncAwaitBestPractices.MVVM;
using System;
using System.Threading.Tasks;
using TPUM.Shared.Connectivity;
using TPUM.Shared.ViewModel;

namespace TPUM.Client.ViewModel
{
    public class ClientVM : ConnectionViewModel
    {
        private Socket _socket;

        public override bool CanDisconnect => _socket?.IsConnected ?? false;
        public override bool CanConnect => !CanDisconnect && Uri.TryCreate(ServerAddress, UriKind.Absolute, out Uri _);

        public ClientVM() : base()
        {
            ConnectCommand = new AsyncCommand(Connect);
            DisconnectCommand = new AsyncCommand(Disconnect);
        }

        private async Task Connect()
        {
            if (!Uri.TryCreate(ServerAddress, UriKind.Absolute, out Uri url))
            {
                await Task.CompletedTask;
            }
            _socket = new Socket(url, _repository);
            await _socket.Start();
            OnPropertyChanged(nameof(CanDisconnect));
            OnPropertyChanged(nameof(CanConnect));
        }

        private async Task Disconnect()
        {
            await Task.Run(() => { _socket?.Stop(); _socket?.Dispose(); });
        }
    }
}
