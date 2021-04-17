using AsyncAwaitBestPractices.MVVM;
using System;
using System.Threading.Tasks;
using TPUM.Shared.Connectivity;
using TPUM.Shared.ViewModel;

namespace TPUM.Server.ViewModel
{
    public class ServerVM : ConnectionViewModel
    {
        private HttpServer _server;

        public override bool CanDisconnect => _server?.IsListening ?? false;
        public override bool CanConnect => !CanDisconnect && Uri.TryCreate(ServerAddress, UriKind.Absolute, out Uri _);

        public ServerVM() : base(null)
        {
            ConnectCommand = new AsyncCommand(Connect);
            DisconnectCommand = new AsyncCommand(Disconnect);
        }

        private async Task Connect()
        {
            if (!Uri.TryCreate(ServerAddress, UriKind.Absolute, out Uri url) || !CanConnect)
            {
                return;
            }
            _server = new HttpServer(url, _repository);
            try
            {
                await _server.Start();
            }
            catch (Exception)
            {

            }
            OnPropertyChanged(nameof(CanDisconnect));
            OnPropertyChanged(nameof(CanConnect));
        }

        private async Task Disconnect()
        {
            try 
            {
                await Task.Run(() => _server?.Stop());
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}
