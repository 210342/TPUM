using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using TPUM.Server.Logic.Core;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Server.WebPresentation
{
    public class WebSocketResponseHandler : IWebSocketResponseHandler
    {
        private const int BUFFER_SIZE = 1024;

        private readonly HttpListenerContext _context;
        private readonly CancellationToken _token;

        private ArraySegment<byte> _buffer;
        private HttpListenerWebSocketContext _wsContext;

        public WebSocketResponseHandler(HttpListenerContext context, CancellationToken token)
        {
            _context = context;
            _token = token;
        }

        public event Action OnClosing;

        public void Handle()
        {
            Task.Run(WebSocketLoop);
        }

        public bool SendEntity(INetworkPacket packet, Func<INetworkPacket, byte[]> serializer)
        {
            foreach ((Memory<byte> chunk, bool last) in SplitObjectIntoBufferSizedChunks(packet, serializer))
            {
                chunk.CopyTo(_buffer.Array.AsMemory());
                try
                {
                    _wsContext.WebSocket?.SendAsync(_buffer, WebSocketMessageType.Binary, last, _token);
                }
                catch (WebSocketException ex)
                {
                    _wsContext.WebSocket?.CloseAsync(WebSocketCloseStatus.InternalServerError, ex.Message, _token);
                    return false;
                }
            }
            return true;
        }

        private static IEnumerable<(Memory<byte> chunk, bool last)> SplitObjectIntoBufferSizedChunks(
            INetworkPacket entity,
            Func<INetworkPacket, byte[]> serializer)
        {
            byte[] fullArray = serializer.Invoke(entity);
            int chunksCount = (int)Math.Ceiling((decimal)fullArray.Length / BUFFER_SIZE);
            byte[] appendedArray = new byte[chunksCount * BUFFER_SIZE];
            Memory<byte> memory = appendedArray.AsMemory();
            fullArray.CopyTo(memory);
            if (fullArray.Length != memory.Length)
            {
                appendedArray[fullArray.Length] = 0x03;
            }
            for (int i = 0; i < chunksCount; ++i)
            {
                bool lastChunk = i == chunksCount - 1;
                yield return (memory.Slice(i * BUFFER_SIZE, BUFFER_SIZE), lastChunk);
            }
        }

        private async Task WebSocketLoop()
        {
            _wsContext = await _context.AcceptWebSocketAsync(null).ConfigureAwait(false);
            _buffer = WebSocket.CreateServerBuffer(BUFFER_SIZE);
            ArraySegment<byte> buffer = WebSocket.CreateClientBuffer(BUFFER_SIZE, BUFFER_SIZE);
            while (_wsContext.WebSocket.State == WebSocketState.Open)
            {
                if (_token.IsCancellationRequested)
                {
                    await _wsContext.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, _token).ConfigureAwait(false);
                    Close();
                    return;
                }
                try
                {
                    WebSocketReceiveResult response = await _wsContext.WebSocket.ReceiveAsync(buffer, _token).ConfigureAwait(false);
                    if (response.MessageType == WebSocketMessageType.Close)
                    {
                        Close();
                        return;
                    }
                }
                catch (WebSocketException ex)
                {
                    if (_wsContext.WebSocket?.State == WebSocketState.Open)
                    {
                        await _wsContext.WebSocket?.CloseAsync(WebSocketCloseStatus.InternalServerError, ex.Message, _token);
                    }
                    Close();
                    return;
                }
            }
            Close();
        }

        private void Close()
        {
            OnClosing?.Invoke();
            Dispose();
        }

        #region IDisposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _wsContext?.WebSocket?.Dispose();
                    OnClosing = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~WebSocketResponseHandler()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
