using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPUM.Shared.Model.Core;
using TPUM.Shared.Model.Entities;

namespace TPUM.Shared.Model
{
    public class Repository : Observable<Entity>, IRepository, IObserver<Entity>
    {
        private readonly object _booksLock = new object();
        private readonly object _authorsLock = new object();

        private readonly IDisposable _dataContextSubscription;
        private DataContext _dataContext;

        public Repository() : this(DataContext.GetExampleContext()) { }

        public Repository(IDataContext dataContext)
        {
            _dataContext = dataContext as DataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _dataContextSubscription = _dataContext.Subscribe(this);
        }

        #region Repository

        public Author AddAuthor(Author author)
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
                        Book original = _dataContext.Books.FirstOrDefault(b => b.Id == author.Books[i].Id);
                        if (original != null)
                        {
                            author.Books[i] = original;
                        }
                    }
                    foreach (Book book in author.Books.Where(b => !_dataContext.Books.Select(a => a.Id).Contains(b.Id)))
                    {
                        _dataContext.BooksCollection.Add(book);
                    }
                }
                _dataContext.AuthorsCollection.Add(author);
            }
            return author;
        }

        public Book AddBook(Book book)
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
                    Author original = _dataContext.Authors.FirstOrDefault(b => b.Id == book.Authors[i].Id);
                    if (original != null)
                    {
                        book.Authors[i] = original;
                    }
                }
                foreach (Author author in book.Authors.Where(b => !_dataContext.Authors.Select(a => a.Id).Contains(b.Id)))
                {
                    _dataContext.AuthorsCollection.Add(author);
                }
                _dataContext.BooksCollection.Add(book);
            }
            return book;
        }

        public object AddEntity(object entity)
        {
            if (!(entity is Entity) || _dataContext is null)
            {
                return null;
            }
            else if (entity is Book book)
            {
                return AddBook(book);
            }
            else if (entity is Author author)
            {
                return AddAuthor(author);
            }
            return null;
        }

        public List<Book> GetBooks() => _dataContext.Books.ToList();

        public List<Author> GetAuthors() => _dataContext.Authors.ToList();

        public async Task<List<Book>> GetBooksAsync() => await Task.Run(GetBooks);

        public async Task<List<Author>> GetAuthorsAsync() => await Task.Run(GetAuthors);

        public Author GetAuthorById(int id) => _dataContext.Authors.FirstOrDefault(a => a.Id == id);

        public Book GetBookById(int id) => _dataContext.Books.FirstOrDefault(b => b.Id == id);

        public void UpdateBooks(List<Book> books)
        {
            lock(_booksLock)
            {
                _dataContext.BooksCollection.Clear();
                books.ForEach(b => _dataContext.BooksCollection.Add(b));
            }
        }

        public void UpdateAuthors(List<Author> authors)
        {
            lock (_authorsLock)
            {
                _dataContext.AuthorsCollection.Clear();
                authors.ForEach(b => _dataContext.AuthorsCollection.Add(b));
            }
        }

        #endregion

        #region IObserver

        public virtual void OnCompleted() { }

        public virtual void OnError(Exception error) { }

        public virtual void OnNext(Entity value)
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
