using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Client.ViewModel
{
    public class StockViewModel : ViewModel, IObserver<IEntity>, IDisposable
    {
        private readonly IRepository _repository;
        private readonly IDisposable _socketSubscription;

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

        public StockViewModel(IDispatcher dispatcher) 
            : this(Logic.Factory.CreateRepository(new Uri("http://localhost:5000/"), Format.JSON, Encoding.UTF8), dispatcher) { }

        public StockViewModel(IRepository repository, IDispatcher dispatcher) : base(dispatcher)
        {
            _repository = repository;
            _socketSubscription = _repository.Subscribe(this);
            Task.Run(() =>_repository.GetAuthors().ToList().ForEach(a => OnNext(a)));
            Task.Run(() => _repository.GetBooks().ToList().ForEach(b => OnNext(b)));
            AddAuthorCommand = new Command(args => Task.Run(_repository.AddRandomAuthor));
        }

        #region IObserver

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(IEntity value)
        {
            _dispatcher?.Invoke(() =>
            {
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
