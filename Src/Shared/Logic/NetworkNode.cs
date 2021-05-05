using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Shared.Logic.Core
{
    public abstract class NetworkNode : Observable<IEntity>, INetworkNode
    {
        protected internal readonly int _bufferSize = 1024;
        protected internal readonly ISerializer<IEnumerable<IEntity>> _entityListSerializer;
        protected internal CancellationTokenSource _cancellationTokenSource;

        public ISerializer<INetworkPacket> Serializer { get; }

        public NetworkNode() : this(Format.JSON, Encoding.Default) { }

        public NetworkNode(Format format) : this(format, Encoding.Default) { }

        public NetworkNode(Encoding encoding) : this(Format.JSON, encoding) { }

        public NetworkNode(Format format, Encoding encoding)
        {
            _entityListSerializer = new Serializer<IEnumerable<IEntity>>(encoding, format);
            Serializer = new Serializer<INetworkPacket>(encoding, format);
        }

        public virtual Task Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            return Task.CompletedTask;
        }

        public virtual void Stop()
        {
            Stop(0);
        }

        public virtual void Stop(int delay)
        {
            _cancellationTokenSource?.CancelAfter(delay);
        }

        public abstract Task<IEnumerable<IBook>> GetBooksAsync();
        public abstract Task<IEnumerable<IAuthor>> GetAuthorsAsync();

        #region IDisposable

        private bool _disposedValue;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (!_cancellationTokenSource.IsCancellationRequested)
                    {
                        _cancellationTokenSource.Cancel();
                        _cancellationTokenSource.Dispose();
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
