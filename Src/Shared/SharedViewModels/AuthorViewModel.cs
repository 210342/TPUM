using System.Collections.Generic;
using System.Linq;
using TPUM.Shared.Model.Entities;

namespace TPUM.Shared.ViewModel
{
    public class AuthorViewModel : ViewModel
    {
        private Author _author;
        public Author Author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<BookViewModel> BookVMs => Author.Books.Select(b => new BookViewModel(b));

        public AuthorViewModel()
        {

        }

        public AuthorViewModel(Author author)
        {
            Author = author;
        }
    }
}
