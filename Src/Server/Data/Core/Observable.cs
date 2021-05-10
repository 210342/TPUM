using System;
using System.Collections.Generic;

namespace TPUM.Server.Data
{
    public abstract class Observable<T> : IObservable<T>, IDisposable
    {
        private event Action<T> EntityChanged;
        private event Action<IEnumerable<T>> EntitiesChanged;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return new Subscription(this, observer);
        }

        protected void InvokeEntityChanged(T entity)
        {
            EntityChanged?.Invoke(entity);
        }

        protected void InvokeEntitiesChanged(IEnumerable<T> entities)
        {
            EntitiesChanged?.Invoke(entities);
        }

        private class Subscription : IDisposable
        {
            private Observable<T> Observed { get; }
            private IObserver<T> Observer { get; }

            internal Subscription(Observable<T> observed, IObserver<T> observer)
            {
                Observed = observed;
                Observer = observer;
                Observed.EntityChanged += Observer.OnNext;
                Observed.EntitiesChanged += EntitiesChanged;
            }

            private void EntitiesChanged(IEnumerable<T> entities)
            {
                foreach (T entity in entities)
                {
                    Observer.OnNext(entity);
                }
            }

            public void Dispose()
            {
                Observed.EntityChanged -= Observer.OnNext;
                Observed.EntitiesChanged -= EntitiesChanged;
            }
        }

        #region IDisposable

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    EntityChanged = null;
                    EntitiesChanged = null;
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

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
