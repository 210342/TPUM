using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPUM.Shared.Model;
using TPUM.Shared.Model.Core;
using TPUM.Shared.Model.Formatters;

namespace TPUM.Shared.Connectivity.Core
{
    public abstract class NetworkNode : Observable<NetworkEntity>, INetworkNode
    {
        protected internal readonly int _bufferSize = 1024;
        protected internal CancellationTokenSource _cancellationTokenSource;

        public ISerializer<NetworkEntity> Serializer { get; }

        public NetworkNode() : this (Format.JSON, Encoding.Default) { }

        public NetworkNode(Format format) : this(format, Encoding.Default) { }

        public NetworkNode(Encoding encoding) : this(Format.JSON, encoding) { }

        public NetworkNode(Format format, Encoding encoding)
        {
            Serializer = new Serializer<NetworkEntity>(encoding, FormatterFactory.CreateFormatter<NetworkEntity>(format));
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
