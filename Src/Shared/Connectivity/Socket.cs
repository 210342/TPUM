using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPUM.Shared.Connectivity.Core;
using TPUM.Shared.Model;
using TPUM.Shared.Model.Core;

namespace TPUM.Shared.Connectivity
{
    public class Socket : NetworkNode
    {
        private readonly ClientWebSocket _webSocket;
        private readonly IRepository _repository;

        public Uri ServerUri { get; }
        public bool IsConnected => _webSocket?.State == WebSocketState.Open;

        public Socket(Uri url) : this(url, null) { }

        public Socket(Uri url, IRepository repository)
        {
            _webSocket = new ClientWebSocket();
            ServerUri = new Uri($"ws://{url}/connect/");
            _repository = repository;
        }

        #region NetworkNode

        public override async Task Start()
        {
            _ = base.Start();
            try
            {
                await _webSocket?.ConnectAsync(ServerUri, _cancellationTokenSource.Token);
                await WebSocketLoop(_cancellationTokenSource.Token);
            }
            catch (WebSocketException)
            {
                return;
            }
        }

        private async Task WebSocketLoop(CancellationToken token)
        {
            ArraySegment<byte> buffer = WebSocket.CreateClientBuffer(_bufferSize, _bufferSize);
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
            List<byte> receivedBytes = new List<byte>(_bufferSize);
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
            string json = Encoding.UTF8.GetString(receivedBytes.ToArray());
            NetworkEntity networkEntity = NetworkEntity.Deserialize(json);
            InvokeEntityChanged(networkEntity);
            _repository.AddEntity(networkEntity.Entity);
        }

        #endregion

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
