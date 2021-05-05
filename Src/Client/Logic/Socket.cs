using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Client.Logic
{
    internal class Socket : NetworkNode
    {
        private readonly ClientWebSocket _webSocket;
        private readonly IRepository _repository;
        private readonly Uri _wsConnectionEndpoint;

        public Uri ServerUri { get; }
        public bool IsConnected => _webSocket?.State == WebSocketState.Open;

        public Socket(Uri serverUri, Format format, Encoding encoding) : this(serverUri, null, format, encoding) { }

        public Socket(Uri serverUri, IRepository repository, Format format, Encoding encoding) : base(format, encoding)
        {
            _ = serverUri ?? throw new ArgumentNullException(nameof(serverUri));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _webSocket = new ClientWebSocket();
            ServerUri = new Uri($"ws://{serverUri.Host}");
            _wsConnectionEndpoint = new Uri($"{ServerUri}/connect");
        }

        #region NetworkNode

        public override async Task Start()
        {
            _ = base.Start();
            try
            {
                await _webSocket?.ConnectAsync(_wsConnectionEndpoint, _cancellationTokenSource.Token);
                await WebSocketLoop(_cancellationTokenSource.Token);
            }
            catch (WebSocketException)
            {
                return;
            }
        }

        public override Task<IEnumerable<IBook>> GetBooksAsync()
        {
            return _repository?.GetBooksAsync() ?? Task.FromResult(Enumerable.Empty<IBook>());
        }

        public override Task<IEnumerable<IAuthor>> GetAuthorsAsync()
        {
            return _repository?.GetAuthorsAsync() ?? Task.FromResult(Enumerable.Empty<IAuthor>());
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
            INetworkPacket networkEntity = AbstractNetworkPacket.Deserialize(receivedBytes.ToArray(), Serializer);
            InvokeEntityChanged(networkEntity.Entity as IEntity);
            _repository?.AddEntity(networkEntity.Entity);
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
