using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPUM.Server.Logic.Core;
using TPUM.Shared.Logic;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Server.Logic
{
    internal class HttpServer : NetworkNode, IObserver<IEntity>
    {
        private readonly IRepository _repository;
        private readonly IDisposable _dataContextSubscription;
        private readonly HttpListener _httpListener;
        private readonly List<IWebSocketResponseHandler> _webSocketSubscribers = new List<IWebSocketResponseHandler>();
        private readonly Func<HttpListenerContext, IRepository, IHttpResponseHandler> _httpHandlerFactory;
        private readonly Func<HttpListenerContext, CancellationToken, IWebSocketResponseHandler> _webSocketHandlerFactory;

        public Uri BaseUri { get; }

        public HttpServer(
            Uri uri,
            IRepository repository,
            Func<HttpListenerContext, IRepository, IHttpResponseHandler> httpHandlerFactory,
            Func<HttpListenerContext, CancellationToken, IWebSocketResponseHandler> webSocketHandlerFactory)
            : this(uri, repository, httpHandlerFactory, webSocketHandlerFactory, Format.JSON, Encoding.UTF8) 
        { }

        public HttpServer(
            Uri uri,
            IRepository repository,
            Func<HttpListenerContext, IRepository, IHttpResponseHandler> httpHandlerFactory,
            Func<HttpListenerContext, CancellationToken, IWebSocketResponseHandler> webSocketHandlerFactory,
            Format format,
            Encoding encoding) 
            : base(format, encoding)
        {
            _ = uri ?? throw new ArgumentNullException(nameof(uri));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _httpHandlerFactory = httpHandlerFactory ?? throw new ArgumentNullException(nameof(httpHandlerFactory));
            _webSocketHandlerFactory = webSocketHandlerFactory ?? throw new ArgumentNullException(nameof(webSocketHandlerFactory));
            _dataContextSubscription = _repository.Subscribe(this);
            _httpListener = new HttpListener();
            BaseUri = uri;
            _httpListener.Prefixes.Add($"{uri}");
            _httpListener.Prefixes.Add($"{uri}books/");
            _httpListener.Prefixes.Add($"{uri}authors/");
            _httpListener.Prefixes.Add($"{uri}add/");
            _httpListener.Prefixes.Add($"{uri}disconnect/");
        }

        public override Task Start()
        {
            base.Start();
            _httpListener.Start();
            Task.Run(() => Start(_cancellationTokenSource.Token));
            _repository.StartBackgroundWorker();
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

        public override Task<IEnumerable<IBook>> GetBooksAsync()
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<IAuthor>> GetAuthorsAsync()
        {
            throw new NotImplementedException();
        }

        private async Task ListenerLoop(CancellationToken token)
        {
            while (_httpListener.IsListening && !token.IsCancellationRequested)
            {
                HttpListenerContext context = await _httpListener.GetContextAsync().ConfigureAwait(false);
                if (context.Request.IsWebSocketRequest)
                {
                    IWebSocketResponseHandler webSocket = _webSocketHandlerFactory.Invoke(context, token);
                    webSocket.OnClosing += () => _webSocketSubscribers.Remove(webSocket);
                    _webSocketSubscribers.Add(webSocket);
                    webSocket.Handle();
                }
                else if (!_httpHandlerFactory
                    .Invoke(context, _repository)
                    .Handle(
                        entity => CreateNetworkPacket(entity, BaseUri).Serialize(),
                        entities => 
                        _entityListSerializer.Serialize(entities.Select(e => Mapper.MapWebModelToDataModelEntities(e)).ToArray())))
                {
                    Stop();
                }
            }
        }

        #region IObserver

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(IEntity value)
        {
            foreach (IWebSocketResponseHandler socket in _webSocketSubscribers)
            {
                socket.SendEntity(CreateNetworkPacket(value, BaseUri), entity => entity?.Serialize());
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
                    _webSocketSubscribers.ForEach(webSocket =>
                    {
                        webSocket.Dispose();
                    });
                    _webSocketSubscribers.Clear();
                    if (_httpListener.IsListening)
                    {
                        _httpListener.Stop();
                    }
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
