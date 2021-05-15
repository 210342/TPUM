using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPUM.Client.Data.Core;
using TPUM.Shared.NetworkModel.Core;

namespace TPUM.Client.Data
{
    internal class Socket : Observable<IEntity>, ISocket
    {
        private const int BUFFER_SIZE = 1024;
        private readonly ClientWebSocket _webSocket;
        private readonly Uri _wsConnectionEndpoint;
        private CancellationTokenSource _cancellationTokenSource;

        public Uri ServerUri { get; }
        public bool IsConnected => _webSocket?.State == WebSocketState.Open;
        internal ISerializer<INetworkPacket> Serializer { get; }

        public Socket(Uri serverUri, Format format, Encoding encoding)
        {
            _ = serverUri ?? throw new ArgumentNullException(nameof(serverUri));
            _ = encoding ?? throw new ArgumentNullException(nameof(encoding));
            _webSocket = new ClientWebSocket();
            Serializer = Shared.NetworkModel.Factory.CreateSerializer<INetworkPacket>(format, encoding);
            ServerUri = new Uri($"ws://{serverUri.Host}");
            _wsConnectionEndpoint = new Uri($"{ServerUri}/connect");
        }

        public async Task Start()
        {
            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                await _webSocket?.ConnectAsync(_wsConnectionEndpoint, _cancellationTokenSource.Token);
                await WebSocketLoop(_cancellationTokenSource.Token);
            }
            catch (WebSocketException)
            {
                return;
            }
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
        }

        private async Task WebSocketLoop(CancellationToken token)
        {
            ArraySegment<byte> buffer = WebSocket.CreateClientBuffer(BUFFER_SIZE, BUFFER_SIZE);
            while (_webSocket.State == WebSocketState.Open)
            {
                if (token.IsCancellationRequested)
                {
                    await _webSocket?.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, token);
                    return;
                }
                try
                {
                    await ReadEntity(buffer, token);
                }
                catch (WebSocketException ex)
                {
                    if (_webSocket?.State == WebSocketState.Open)
                    {
                        await _webSocket?.CloseAsync(WebSocketCloseStatus.InternalServerError, ex.Message, token);
                    }
                }
            }
        }

        private async Task ReadEntity(ArraySegment<byte> buffer, CancellationToken token)
        {
            List<byte> receivedBytes = new List<byte>(BUFFER_SIZE);
            WebSocketReceiveResult response;
            do
            {
                response = await _webSocket?.ReceiveAsync(buffer, token);
                receivedBytes.AddRange(buffer);
            }
            while (!response.EndOfMessage);
            int endIndex = receivedBytes.LastIndexOf(0x03);
            if (endIndex > 0)
            {
                receivedBytes.RemoveRange(endIndex, receivedBytes.Count - endIndex);
            }
            INetworkPacket networkEntity = AbstractNetworkPacket.Deserialize(receivedBytes.ToArray(), Serializer);
            InvokeEntityChanged(networkEntity.Entity as IEntity);
        }

        #region IDisposable

        private bool _disposedValue;

        protected override void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _webSocket?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
            base.Dispose(disposing);
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Socket()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public new void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
