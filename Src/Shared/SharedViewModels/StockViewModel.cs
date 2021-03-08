using System;
using System.Collections.ObjectModel;
using System.Linq;
using TPUM.Model;
using TPUM.Model.Core;
using TPUM.Model.Entities;

namespace TPUM.Shared.ViewModel
{
    public class StockViewModel : ViewModel, IObserver<Entity>, IDisposable
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

        public StockViewModel(IRepository repository)
        {
            _repository = repository;
            _socketSubscription = _repository.Subscribe(this);
            _repository.GetAuthors().ForEach(a => Authors.Add(new AuthorViewModel(a)));
            _repository.GetBooks().ForEach(b => Books.Add(new BookViewModel(b)));
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(Entity value)
        {
            if (value is Book book)
            {
                Books.Remove(Books.FirstOrDefault(b => b.Book.Id == book.Id));
                Books.Add(new BookViewModel(book));
            }
            else if (value is Author author)
            {
                Authors.Remove(Authors.FirstOrDefault(a => a.Author.Id == author.Id));
                Authors.Add(new AuthorViewModel(author));
            }
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
