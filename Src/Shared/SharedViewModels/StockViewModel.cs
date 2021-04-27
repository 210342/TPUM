using System;
using System.Collections.ObjectModel;
using System.Linq;
using TPUM.Shared.Logic;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.Dto;

namespace TPUM.Shared.ViewModel
{
    public class StockViewModel : ViewModel, IObserver<IEntityDto>, IDisposable
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

        public StockViewModel(IDispatcher dispatcher) : this(LogicFactory.GetExampleRepository(), dispatcher) { }

        public StockViewModel(IRepository repository, IDispatcher dispatcher) : base(dispatcher)
        {
            _repository = repository;
            _socketSubscription = _repository.Subscribe(this);
            _repository.GetAuthors().ForEach(a => Authors.Add(new AuthorViewModel(a)));
            _repository.GetBooks().ForEach(b => Books.Add(new BookViewModel(b)));
            AddAuthorCommand = new Command((object args) => ObjectCreation.AddAuthor(_repository));
            ObjectCreation.AddBook(_repository);
        }

        #region IObserver

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(IEntityDto value)
        {
            _dispatcher?.Invoke(() =>
            {
                if (value is IBookDto book)
                {
                    Books.Remove(Books.FirstOrDefault(b => b.Book.EntityId == book.EntityId));
                    Books.Add(new BookViewModel(book));
                }
                else if (value is IAuthorDto author)
                {
                    Authors.Remove(Authors.FirstOrDefault(a => a.Author.EntityId == author.EntityId));
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
