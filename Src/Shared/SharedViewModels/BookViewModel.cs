using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<AuthorViewModel> AuthorVMs => Book?.Authors.Select(a => new AuthorViewModel(a));

        public BookViewModel()
        {

        }

        public BookViewModel(Book book)
        {
            Book = book;
        }
    }
}
