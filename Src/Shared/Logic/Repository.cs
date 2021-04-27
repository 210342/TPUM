using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPUM.Shared.Data;
using TPUM.Shared.Data.Core;
using TPUM.Shared.Data.Entities;

namespace TPUM.Shared.Logic
{
    internal class Repository : Observable<IEntity>, IRepository, IObserver<IEntity>
    {
        private readonly object _booksLock = new object();
        private readonly object _authorsLock = new object();

        private readonly IDisposable _dataContextSubscription;
        private IDataContext _dataContext;

        public Repository() : this(DataFactory.GetExampleContext()) { }

        public Repository(IDataContext dataContext)
        {
            // TODO: this assertion is no longer needed
            _dataContext = dataContext as IDataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _dataContextSubscription = _dataContext.Subscribe(this);
        }

        #region Repository

        public IAuthor AddAuthor(IAuthor author)
        {
            if (_dataContext is null || author is null)
            {
                return null;
            }

            lock (_authorsLock)
            {
                if (_dataContext.Authors.Select(a => a.Id).Contains(author.Id))
                {
                    return author;
                }

                lock (_booksLock)
                {
                    for (int i = 0; i < author.Books.Count; ++i)
                    {
                        IBook original = _dataContext.Books.FirstOrDefault(b => b.Id == author.Books[i].Id);
                        if (original != null)
                        {
                            author.Books[i] = original;
                        }
                    }
                    foreach (IBook book in author.Books.Where(b => !_dataContext.Books.Select(a => a.Id).Contains(b.Id)))
                    {
                        _dataContext.AddBook(book);
                    }
                }
                _dataContext.AddAuthor(author);
            }
            return author;
        }

        public IBook AddBook(IBook book)
        {
            if (_dataContext is null || book is null)
            {
                return null;
            }

            lock (_authorsLock)
                lock (_booksLock)
                {
                    if (_dataContext.Books.Select(b => b.Id).Contains(book.Id))
                    {
                        return book;
                    }

                    for (int i = 0; i < book.Authors.Count; ++i)
                    {
                        IAuthor original = _dataContext.Authors.FirstOrDefault(b => b.Id == book.Authors[i].Id);
                        if (original != null)
                        {
                            book.Authors[i] = original;
                        }
                    }
                    foreach (IAuthor author in book.Authors.Where(b => !_dataContext.Authors.Select(a => a.Id).Contains(b.Id)))
                    {
                        _dataContext.AddAuthor(author);
                    }
                    _dataContext.AddBook(book);
                }
            return book;
        }

        public object AddEntity(object entity)
        {
            if (!(entity is IEntity) || _dataContext is null)
            {
                return null;
            }
            else if (entity is IBook book)
            {
                return AddBook(book);
            }
            else if (entity is IAuthor author)
            {
                return AddAuthor(author);
            }
            return null;
        }

        public List<IBook> GetBooks() => _dataContext.Books.Cast<IBook>().ToList();

        public List<IAuthor> GetAuthors() => _dataContext.Authors.Cast<IAuthor>().ToList();

        public async Task<List<IBook>> GetBooksAsync() => await Task.Run(GetBooks);

        public async Task<List<IAuthor>> GetAuthorsAsync() => await Task.Run(GetAuthors);

        public IAuthor GetAuthorById(int id) => _dataContext.Authors.FirstOrDefault(a => a.Id == id);

        public IBook GetBookById(int id) => _dataContext.Books.FirstOrDefault(b => b.Id == id);

        public void UpdateBooks(List<IBook> books)
        {
            lock (_booksLock)
            {
                _dataContext.ClearBooks();
                _dataContext.UpdateBooks(books);
            }
        }

        public void UpdateAuthors(List<IAuthor> authors)
        {
            lock (_authorsLock)
            {
                _dataContext.ClearAuthors();
                _dataContext.UpdateAuthors(authors);
            }
        }

        #endregion

        #region IObserver

        public virtual void OnCompleted() { }

        public virtual void OnError(Exception error) { }

        public virtual void OnNext(IEntity value)
        {
            InvokeEntityChanged(value);
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
                    _dataContextSubscription?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _dataContext = null;
                _disposedValue = true;
            }
        }

        #endregion
    }
}
