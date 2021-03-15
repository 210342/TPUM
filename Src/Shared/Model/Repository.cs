using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPUM.Shared.Core.Model;
using TPUM.Shared.Model.Core;
using TPUM.Shared.Model.Entities;

namespace TPUM.Shared.Model
{
    public class Repository : Observable<Entity>, IRepository, IObserver<Entity>
    {
        private readonly object _booksLock = new object();
        private readonly object _authorsLock = new object();

        private readonly IDisposable _dataContextSubscription;
        private readonly DataContext _dataContext;

        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _dataContextSubscription = _dataContext.Subscribe(this);
        }

        #region Repository

        public Author AddAuthor(Author author)
        {
            if (author == null || _dataContext.Authors.Select(a => a.Id).Contains(author.Id))
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
            }
            lock (_authorsLock)
            {
                _dataContext.AuthorsCollection.Add(author);
            }
            lock (_booksLock)
            {
                foreach (Book book in author.Books.Where(b => !_dataContext.Books.Select(a => a.Id).Contains(b.Id)))
                {
                    _dataContext.BooksCollection.Add(book);
                }
            }
            return author;
        }

        public Book AddBook(Book book)
        {
            if (book == null || _dataContext.Books.Select(b => b.Id).Contains(book.Id))
            {
                return book;
            }

            lock (_booksLock)
            {
                for (int i = 0; i < book.Authors.Count; ++i)
                {
                    Author original = _dataContext.Authors.FirstOrDefault(b => b.Id == book.Authors[i].Id);
                    if (original != null)
                    {
                        book.Authors[i] = original;
                    }
                }
            }
            _dataContext.BooksCollection.Add(book);
            lock (_booksLock)
            {
                foreach (Author author in book.Authors.Where(b => !_dataContext.Authors.Select(a => a.Id).Contains(b.Id)))
                {
                    _dataContext.AuthorsCollection.Add(author);
                }
            }
            return book;
        }

        public void AddEntity(object entity)
        {
            if (!(entity is Entity))
            {
                return;
            }
            else if (entity is Book book)
            {
                AddBook(book);
            }
            else if (entity is Author author)
            {
                AddAuthor(author);
            }
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
                _dataContext.BooksCollection.Clear();
                authors.ForEach(b => _dataContext.AuthorsCollection.Add(b));
            }
        }

        #endregion

        #region IObserver

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(Entity value)
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
                _disposedValue = true;
            }
        }

        #endregion
    }
}
