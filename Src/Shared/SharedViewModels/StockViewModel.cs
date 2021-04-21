using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TPUM.Shared.Data.Core;
using TPUM.Shared.Data.Entities;

namespace TPUM.Shared.ViewModel
{
    public class StockViewModel : ViewModel, IObserver<IEntity>, IDisposable
    {
        private readonly IRepository _repository;
        private readonly IDisposable _socketSubscription;
        private readonly Random _rng = new Random();

        #region Observable properties

        private ObservableCollection<BookViewModel> _books = new ObservableCollection<BookViewModel>();

        public ObservableCollection<BookViewModel> Books
        {
            get => _books;
            set
            {
                _books = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AuthorViewModel> _authors = new ObservableCollection<AuthorViewModel>();

        public ObservableCollection<AuthorViewModel> Authors
        {
            get => _authors;
            set
            {
                _authors = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public Command AddAuthorCommand { get; private set; }

        #endregion

        public StockViewModel(IDispatcher dispatcher) : this(DataFactory.CreateObject<IRepository>(), dispatcher) { }

        public StockViewModel(IRepository repository, IDispatcher dispatcher) : base(dispatcher)
        {
            _repository = repository;
            _socketSubscription = _repository.Subscribe(this);
            _repository.GetAuthors().ForEach(a => Authors.Add(new AuthorViewModel(a)));
            _repository.GetBooks().ForEach(b => Books.Add(new BookViewModel(b)));
            AddAuthorCommand = new Command(AddAuthor);
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    int id;
                    do
                    {
                        id = _rng.Next(5000);
                    }
                    while (_repository.GetBookById(id) != null);
                    IBook newBook = DataFactory.CreateObject<IBook>();
                    newBook.Id = id;
                    newBook.Title = $"{id} - {nameof(IBook.Title)}";
                    _repository.AddBook(newBook);
                }
            });
        }

        #region Command implementations

        public void AddAuthor(object args)
        {
            Task.Run(() =>
            {
                int id;
                do
                {
                    id = _rng.Next(5000);
                }
                while (_repository.GetAuthorById(id) != null);

                IAuthor newAuthor = DataFactory.CreateObject<IAuthor>();
                newAuthor.Id = id;
                newAuthor.FirstName = $"{id} - {nameof(IAuthor.FirstName)}";
                newAuthor.LastName = $"{id} - {nameof(IAuthor.LastName)}";
                newAuthor.NickName = $"{id} - {nameof(IAuthor.NickName)}";
                _repository.AddAuthor(newAuthor);
            });
        }

        #endregion

        #region IObserver

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(IEntity value)
        {
            _dispatcher?.Invoke(() => {
                if (value is IBook book)
                {
                    Books.Remove(Books.FirstOrDefault(b => b.Book.Id == book.Id));
                    Books.Add(new BookViewModel(book));
                }
                else if (value is IAuthor author)
                {
                    Authors.Remove(Authors.FirstOrDefault(a => a.Author.Id == author.Id));
                    Authors.Add(new AuthorViewModel(author));
                }
            });
        }

        #endregion

        #region IDisposable

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _socketSubscription.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~StockViewModel()
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
