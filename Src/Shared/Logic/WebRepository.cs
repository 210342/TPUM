using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Shared.Logic
{
    internal class WebRepository : IRepository
    {
        private readonly IDataContext _dataContext;

        public WebRepository(IDataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
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
