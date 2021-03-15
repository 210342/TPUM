using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TPUM.Shared.Connectivity.Core;
using TPUM.Shared.Core.Model;
using TPUM.Shared.Model.Core;
using TPUM.Shared.Model.Entities;

namespace TPUM.Shared.Connectivity
{
    public class HttpServer : NetworkNode, IObserver<Entity>
    {
        private readonly IRepository _repository;
        private readonly IDisposable _dataContextSubscription;
        private readonly HttpListener _httpListener;
        private readonly List<(Thread thread, HttpListenerWebSocketContext context)> _webSocketSubscribers = new List<(Thread thread, HttpListenerWebSocketContext context)>();
        private ArraySegment<byte> _buffer;

        public Uri BaseUri { get; }
        public bool IsListening => _httpListener?.IsListening ?? false;

        public HttpServer(Uri url, IRepository repository) : this(url, repository, Format.JSON, Encoding.UTF8) { }

        public HttpServer(Uri url, IRepository repository, Format format, Encoding encoding) : base(format, encoding)
        {
            _repository = repository;
            _dataContextSubscription = _repository.Subscribe(this);
            _httpListener = new HttpListener();
            BaseUri = new Uri($"http://{url}");
            _httpListener.Prefixes.Add($"http://{url}/books/");
            _httpListener.Prefixes.Add($"http://{url}/authors/");
            _httpListener.Prefixes.Add($"http://{url}/add/");
            _httpListener.Prefixes.Add($"http://{url}/connect/");
            _httpListener.Prefixes.Add($"http://{url}/disconnect/");
        }

        public override Task Start()
        {
            base.Start();
            _httpListener.Start();
            Task.Run(() => Start(_cancellationTokenSource.Token));
            return Task.CompletedTask;
        }

        private async Task Start(CancellationToken token)
        {
            _httpListener.Start();
            await ListenerLoop(token).ConfigureAwait(false);
        }

        public override void Stop()
        {
            base.Stop();
            _httpListener.Close();
        }

        private async Task ListenerLoop(CancellationToken token)
        {
            while (_httpListener.IsListening)
            {
                await Task.Delay(5000).ConfigureAwait(false);
                HttpListenerContext context = await _httpListener.GetContextAsync().ConfigureAwait(false);
                if (context.Request.IsWebSocketRequest)
                {
                    if (_buffer == null)
                    {
                        _buffer = WebSocket.CreateServerBuffer(_bufferSize);
                    }
                    HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null).ConfigureAwait(false);
                    Thread wsThread = new Thread(() => WebSocketLoop(wsContext, token));
                    _webSocketSubscribers.Add((wsThread, wsContext));
                    wsThread.Start();
                }
                else
                {
                    RespondHttp(context).Close();
                }
            }
        }

        private async void WebSocketLoop(HttpListenerWebSocketContext context, CancellationToken token)
        {
            ArraySegment<byte> buffer = WebSocket.CreateClientBuffer(_bufferSize, _bufferSize);
            while (context.WebSocket.State == WebSocketState.Open)
            {
                if (token.IsCancellationRequested)
                {
                    (Thread thread, HttpListenerWebSocketContext context) tuple = _webSocketSubscribers.FirstOrDefault(t => t.context.Equals(context.SecWebSocketKey));
                    _webSocketSubscribers.Remove(tuple);
                    await context.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, token).ConfigureAwait(false);
                    return;
                }
                try
                {
                    WebSocketReceiveResult response = await context.WebSocket.ReceiveAsync(buffer, token).ConfigureAwait(false);
                    if (response.MessageType == WebSocketMessageType.Close)
                    {
                        (Thread thread, HttpListenerWebSocketContext context) tuple = _webSocketSubscribers.FirstOrDefault(t => t.context.Equals(context.SecWebSocketKey));
                        _webSocketSubscribers.Remove(tuple);
                    }
                }
                catch (WebSocketException ex)
                {
                    (Thread thread, HttpListenerWebSocketContext context) tuple = _webSocketSubscribers.FirstOrDefault(t => t.context.Equals(context.SecWebSocketKey));
                    _webSocketSubscribers.Remove(tuple);
                    if (context.WebSocket?.State == WebSocketState.Open)
                    {
                        await context.WebSocket?.CloseAsync(WebSocketCloseStatus.InternalServerError, ex.Message, token);
                    }
                }
            }
        }

        private HttpListenerResponse RespondHttp(HttpListenerContext context)
        {
            HttpListenerResponse response = context.Response;
            response.StatusCode = 200;
            response.ContentType = "text/plain; charset=utf-8";
            using (StreamWriter writer = new StreamWriter(response.OutputStream))
            {
                if (context.Request.RawUrl.ToLower().Contains("disconnect"))
                {
                    writer.WriteLine("Closing the server");
                    Stop();
                }
                else if (context.Request.RawUrl.ToLower().Contains("books"))
                {
                    writer.WriteLine(JsonSerializer.Serialize(_repository.GetBooks(), typeof(List<Book>)));
                }
                else if (context.Request.RawUrl.ToLower().Contains("authors"))
                {
                    writer.WriteLine(JsonSerializer.Serialize(_repository.GetAuthors(), typeof(List<Author>)));
                }
                else if (context.Request.RawUrl.ToLower().Contains("add"))
                {
                    Random rng = new Random();
                    _repository.AddAuthor(new Author()
                    {
                        Id = rng.Next(),
                        FirstName = "added"
                    });
                }
            }
            return response;
        }

        private IEnumerable<(Memory<byte> chunk, bool last)> SplitObjectIntoBufferSizedChunks(NetworkEntity entity)
        {
            byte[] fullArray = entity.Serialize(Serializer);
            int chunksCount = (int)Math.Ceiling((decimal)fullArray.Length / _bufferSize);
            byte[] appendedArray = new byte[chunksCount * _bufferSize];
            Memory<byte> memory = appendedArray.AsMemory();
            fullArray.CopyTo(memory);
            if (fullArray.Length != memory.Length)
            {
                appendedArray[fullArray.Length] = 0x03;
            }
            for (int i = 0; i < chunksCount; ++i)
            {
                bool lastChunk = i == chunksCount - 1;
                yield return (memory.Slice(i * _bufferSize, _bufferSize), lastChunk);
            }
        }

        #region IObserver

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(Entity value)
        {
            if (_buffer == null || !_webSocketSubscribers.Any())
            {
                return;
            }
            NetworkEntity networkEntity = new NetworkEntity()
            {
                Source = BaseUri,
                TypeIdentifier = value.GetType().GUID,
                Entity = value
            };
            foreach ((Memory<byte> chunk, bool last) in SplitObjectIntoBufferSizedChunks(networkEntity))
            {
                chunk.CopyTo(_buffer.Array.AsMemory());
                foreach (HttpListenerWebSocketContext context in _webSocketSubscribers.Select(t => t.context))
                {
                    try
                    {
                        context.WebSocket?.SendAsync(_buffer, WebSocketMessageType.Text, last, _cancellationTokenSource.Token);
                    }
                    catch (WebSocketException ex)
                    {
                        context.WebSocket?.CloseAsync(WebSocketCloseStatus.InternalServerError, ex.Message, _cancellationTokenSource.Token);
                    }
                }
            }
        }

        #endregion

        #region IDisposable

        private bool _disposedValue;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _dataContextSubscription.Dispose();
                    _webSocketSubscribers.ForEach(tuple =>
                    {
                        tuple.context.WebSocket.Dispose();
                        tuple.thread.Join();
                    });
                    _webSocketSubscribers.Clear();
                    _httpListener?.Stop();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~HttpServer()
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
