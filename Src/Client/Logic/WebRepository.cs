using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUM.Client.Data.Core;
using TPUM.Shared.Logic;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Client.Logic
{
    internal class WebRepository : Shared.NetworkModel.Core.Observable<IEntity>, IRepository, IObserver<Shared.NetworkModel.Core.IEntity>
    {
        private readonly ISocket _socket;
        private readonly IHttpClient _httpClient;
        private readonly IDisposable _socketSubscription;

        public WebRepository(Uri uri, Shared.NetworkModel.Core.Format format, Encoding encoding)
        {
            _socket = Data.Factory.CreateWebDataSource<ISocket>(uri, format, encoding);
            _httpClient = Data.Factory.CreateWebDataSource<IHttpClient>(uri, format, encoding);
            _socketSubscription = _socket.Subscribe(this);
            _socket.Start();
        }

        public async Task<IAuthor> AddRandomAuthor()
        {
            try { 
                return Mapper.MapEntities<Shared.NetworkModel.Core.IAuthor, IAuthor>(
                    await _httpClient.AddRandomAuthorAsync()
                );
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public IAuthor GetAuthorById(int id)
        {
            return GetAuthors().FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<IAuthor> GetAuthors()
        {
            return GetAuthorsAsync().GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<IAuthor>> GetAuthorsAsync()
        {
            try 
            {
                return (await _httpClient.GetAuthorsAsync()).Select(a => Mapper.MapEntities<Shared.NetworkModel.Core.IAuthor, IAuthor>(a));
            }
            catch (OperationCanceledException)
            {
                return Enumerable.Empty<IAuthor>();
            }
        }

        public IBook GetBookById(int id)
        {
            return GetBooks().FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<IBook> GetBooks()
        {
            return GetBooksAsync().GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<IBook>> GetBooksAsync()
        {
            try
            { 
                return (await _httpClient.GetBooksAsync()).Select(b => Mapper.MapEntities<Shared.NetworkModel.Core.IBook, IBook>(b));
            }
            catch (OperationCanceledException)
            {
                return Enumerable.Empty<IBook>();
            }
        }

        #region Unimplemented methods

        public IAuthor AddAuthor(IAuthor author)
        {
            throw new NotImplementedException();
        }

        public IBook AddBook(IBook book)
        {
            throw new NotImplementedException();
        }

        public object AddEntity(object entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateAuthors(List<IAuthor> authors)
        {
            throw new NotImplementedException();
        }

        public void UpdateBooks(List<IBook> books)
        {
            throw new NotImplementedException();
        }

        public bool StartBackgroundWorker()
        {
            throw new NotImplementedException("Cannot start a background worker for client-side repository");
        }

        #endregion

        #region IObserver

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(Shared.NetworkModel.Core.IEntity value)
        {
            InvokeEntityChanged(Mapper.MapWebModelToDataModelEntities(value));
        }

        #endregion

        #region IDisposable

        private bool disposedValue;
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _socketSubscription?.Dispose();
                    _socket.Stop();
                    _socket.Dispose();
                    _httpClient.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~WebRepository()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        #endregion
    }
}
