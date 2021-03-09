using TPUM.Shared.Model.Entities;

namespace TPUM.Shared.ViewModel
{
    public class BookViewModel : ViewModel
    {
        private Book _book;
        public Book Book
        {
            get => _book;
            set
            {
                _book = value;
                OnPropertyChanged();
            }
        }

        public BookViewModel()
        {

        }

        public BookViewModel(Book book)
        {
            Book = book;
        }
    }
}
