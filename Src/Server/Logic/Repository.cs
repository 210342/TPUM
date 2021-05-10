using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPUM.Server.Data;
using TPUM.Server.Data.Core;
using TPUM.Server.Data.Entities;
using TPUM.Shared.Logic.Core;

namespace TPUM.Server.Logic
{
    internal class Repository : Observable<Shared.Logic.WebModel.IEntity>, IRepository, IObserver<IEntity>
    {
        private readonly object _booksLock = new object();
        private readonly object _authorsLock = new object();

        private readonly IDisposable _dataContextSubscription;
        private Data.Core.IDataContext _dataContext;

        public Repository() : this(DataFactory.GetExampleContext()) { }

        public Repository(Data.Core.IDataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _dataContextSubscription = _dataContext.Subscribe(this);
        }

        #region Repository

        public Shared.Logic.WebModel.IAuthor AddAuthor(Shared.Logic.WebModel.IAuthor author)
        {
            IAuthor dataAuthor = LogicFactory.MapWebModelToEntity(author) as IAuthor;
            if (_dataContext is null || dataAuthor is null)
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
                    for (int i = 0; i < dataAuthor.Books.Count(); ++i)
                    {
                        IBook original = _dataContext.Books.FirstOrDefault(b => b.Id == dataAuthor.Books[i].Id);
                        if (original != null)
                        {
                            dataAuthor.Books[i] = original;
                        }
                    }
                    foreach (IBook book in dataAuthor.Books.Where(b => !_dataContext.Books.Select(a => a.Id).Contains(b.Id)))
                    {
                        _dataContext.AddBook(book);
                    }
                }
                _dataContext.AddAuthor(dataAuthor);
            }
            return author;
        }

        public Shared.Logic.WebModel.IBook AddBook(Shared.Logic.WebModel.IBook book)
        {
            IBook dataBook = LogicFactory.MapWebModelToEntity(book) as IBook;
            if (_dataContext is null || dataBook is null)
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

                    for (int i = 0; i < dataBook.Authors.Count; ++i)
                    {
                        Data.Entities.IAuthor original = _dataContext.Authors.FirstOrDefault(b => b.Id == dataBook.Authors[i].Id);
                        if (original != null)
                        {
                            dataBook.Authors[i] = original;
                        }
                    }
                    foreach (IAuthor author in dataBook.Authors.Where(b => !_dataContext.Authors.Select(a => a.Id).Contains(b.Id)))
                    {
                        _dataContext.AddAuthor(author);
                    }
                    _dataContext.AddBook(dataBook);
                }
            return book;
        }

        public object AddEntity(object entity)
        {
            if (!(entity is Shared.Logic.WebModel.IEntity) || _dataContext is null)
            {
                return null;
            }
            else if (entity is Shared.Logic.WebModel.IBook book)
            {
                return AddBook(book);
            }
            else if (entity is Shared.Logic.WebModel.IAuthor author)
            {
                return AddAuthor(author);
            }
            return null;
        }

        public IEnumerable<Shared.Logic.WebModel.IBook> GetBooks() => _dataContext.Books.Select(b => LogicFactory.MapEntityToWebModel(b) as Shared.Logic.WebModel.IBook).ToList();

        public IEnumerable<Shared.Logic.WebModel.IAuthor> GetAuthors() => _dataContext.Authors.Select(a => LogicFactory.MapEntityToWebModel(a) as Shared.Logic.WebModel.IAuthor).ToList();

        public async Task<IEnumerable<Shared.Logic.WebModel.IBook>> GetBooksAsync() => await Task.Run(GetBooks);

        public async Task<IEnumerable<Shared.Logic.WebModel.IAuthor>> GetAuthorsAsync() => await Task.Run(GetAuthors);

        public Shared.Logic.WebModel.IAuthor GetAuthorById(int id)
        {
            IAuthor author = _dataContext.Authors.FirstOrDefault(a => a.Id == id);
            if (author != null)
            {
                return LogicFactory.MapEntityToWebModel(author) as Shared.Logic.WebModel.IAuthor;
            }
            return default;
        }

        public Shared.Logic.WebModel.IBook GetBookById(int id)
        {
            IBook book = _dataContext.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                return LogicFactory.MapEntityToWebModel(book) as Shared.Logic.WebModel.IBook;
            }
            return default;
        }

        public void UpdateBooks(List<Shared.Logic.WebModel.IBook> books)
        {
            lock (_booksLock)
            {
                _dataContext.ClearBooks();
                _dataContext.UpdateBooks(books.Select(b => LogicFactory.MapWebModelToEntity(b) as IBook));
            }
        }

        public void UpdateAuthors(List<Shared.Logic.WebModel.IAuthor> authors)
        {
            lock (_authorsLock)
            {
                _dataContext.ClearAuthors();
                _dataContext.UpdateAuthors(authors.Select(a => LogicFactory.MapWebModelToEntity(a) as IAuthor));
            }
        }

        #endregion

        #region IObserver

        public virtual void OnCompleted() { }

        public virtual void OnError(Exception error) { }

        public virtual void OnNext(IEntity value)
        {
            InvokeEntityChanged(value as Shared.Logic.WebModel.IEntity ?? LogicFactory.MapEntityToWebModel(value));
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

        public Shared.Logic.WebModel.IAuthor AddRandomAuthor()
        {
            return LogicFactory.MapEntityToWebModel(ObjectCreation.AddAuthor(_dataContext)) as Shared.Logic.WebModel.IAuthor;
        }

        internal void StartBackgroundWorker()
        {
            ObjectCreation.AddBookInLoop(_dataContext);
        }
    }
}
