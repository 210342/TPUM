using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPUM.Shared.Data;
using TPUM.Shared.Data.Core;
using TPUM.Shared.Data.Entities;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.Dto;

namespace TPUM.Shared.Logic
{
    internal class Repository : Observable<IEntityDto>, IRepository, IObserver<IEntity>
    {
        private readonly object _booksLock = new object();
        private readonly object _authorsLock = new object();

        private readonly IDisposable _dataContextSubscription;
        private IDataContext _dataContext;

        public Repository() : this(DataFactory.GetExampleContext()) { }

        public Repository(IDataContext dataContext)
        {
            // TODO: this assertion is no longer needed
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _dataContextSubscription = _dataContext.Subscribe(this);
        }

        #region Repository

        public IAuthorDto AddAuthor(IAuthorDto author)
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
                    foreach (IBookDto book in author.Books.Where(b => !_dataContext.Books.Select(a => a.Id).Contains(b.Id)))
                    {
                        _dataContext.AddBook(book);
                    }
                }
                _dataContext.AddAuthor(author);
            }
            return author;
        }

        public IBookDto AddBook(IBookDto book)
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
            else if (entity is IBookDto book)
            {
                return AddBook(book);
            }
            else if (entity is IAuthorDto author)
            {
                return AddAuthor(author);
            }
            return null;
        }

        public List<IBookDto> GetBooks() => _dataContext.Books.Select(b => new BookDto(b) as IBookDto).ToList();

        public List<IAuthorDto> GetAuthors() => _dataContext.Authors.Select(a => new AuthorDto(a) as IAuthorDto).ToList();

        public async Task<List<IBookDto>> GetBooksAsync() => await Task.Run(GetBooks);

        public async Task<List<IAuthorDto>> GetAuthorsAsync() => await Task.Run(GetAuthors);

        public IAuthorDto GetAuthorById(int id)
        {
            IAuthor author = _dataContext.Authors.FirstOrDefault(a => a.Id == id);
            if (author != null)
            {
                return new AuthorDto(author);
            }
            return default;
        }

        public IBookDto GetBookById(int id)
        {
            IBook book = _dataContext.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                return new BookDto(book);
            }
            return default;
        }

        public void UpdateBooks(List<IBookDto> books)
        {
            lock (_booksLock)
            {
                _dataContext.ClearBooks();
                _dataContext.UpdateBooks(books);
            }
        }

        public void UpdateAuthors(List<IAuthorDto> authors)
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
            InvokeEntityChanged(value as IEntityDto ?? LogicFactory.CreateDtoObjectForEntity(value) as IEntityDto);
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
