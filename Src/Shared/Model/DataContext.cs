using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using TPUM.Shared.Model.Core;
using TPUM.Shared.Model.Entities;

namespace TPUM.Shared.Model
{
    public class DataContext : Observable<Entity>
    {
        private readonly ObservableCollection<Author> _authors = new ObservableCollection<Author>();
        private readonly ObservableCollection<Book> _books = new ObservableCollection<Book>();

        internal ICollection<Book> BooksCollection => _books;
        internal ICollection<Author> AuthorsCollection => _authors;

        public IReadOnlyCollection<Book> Books => _books;
        public IReadOnlyCollection<Author> Authors => _authors;

        public DataContext()
        {
            _authors.CollectionChanged += CollectionChanged;
            _books.CollectionChanged += CollectionChanged;
        }

        private DataContext(List<Author> authors, List<Book> books)
        {
            authors.ForEach(a => _authors.Add(a));
            books.ForEach(b => _books.Add(b));
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
            => InvokeEntitiesChanged(args.NewItems.OfType<Entity>());

        #region IDisposable

        private bool _disposedValue;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _authors.CollectionChanged -= CollectionChanged;
                    _books.CollectionChanged -= CollectionChanged;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        #endregion

        public static DataContext ExampleContext()
        {
            Book book1 = new Book()
            {
                Id = 1,
                Title = "Title1"
            };
            Author author1 = new Author()
            {
                Id = 2,
                FirstName = "FirstName1",
                LastName = "LastName1",
                Books = new List<Book>() { book1 }
            };
            Author author2 = new Author()
            {
                Id = 3,
                FirstName = "FirstName2",
                LastName = "LastName2",
                Books = new List<Book>() { book1 }
            };
            Book book2 = new Book()
            {
                Id = 4,
                Title = "title2",
                Authors = new List<Author>() { author1 }
            };
            Book book3 = new Book()
            {
                Id = 5,
                Title = "title3",
                Authors = new List<Author>() { author2 }
            };

            return new DataContext(
                new List<Author>() { author1, author2 },
                new List<Book>() { book1, book2, book3 }
            );
        }
    }
}
