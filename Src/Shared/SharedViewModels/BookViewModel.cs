using System.Collections.Generic;
using System.Linq;
using TPUM.Shared.Data.Entities;

namespace TPUM.Shared.ViewModel
{
    public class BookViewModel : ViewModel
    {
        private IBook _book;
        public IBook Book
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

        public BookViewModel(IBook book)
        {
            Book = book;
        }
    }
}
