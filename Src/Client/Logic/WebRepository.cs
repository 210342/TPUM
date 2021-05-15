using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TPUM.Client.Data.Core;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Client.Logic
{
    internal class WebRepository : IRepository
    {
        private readonly ISocket _socket;
        private readonly IHttpClient _httpClient;

        public WebRepository(Uri uri, Shared.NetworkModel.Core.Format format, Encoding encoding)
        {
            _socket = Data.Factory.CreateWebDataSource<ISocket>(uri, format, encoding);
            _httpClient = Data.Factory.CreateWebDataSource<IHttpClient>(uri, format, encoding);
        }

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

        public IAuthor AddRandomAuthor()
        {
            throw new NotImplementedException();
        }

        public IAuthor GetAuthorById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAuthor> GetAuthors()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IAuthor>> GetAuthorsAsync()
        {
            throw new NotImplementedException();
        }

        public IBook GetBookById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IBook> GetBooks()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IBook>> GetBooksAsync()
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<IEntity> observer)
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

        #region IDisposable

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
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

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
