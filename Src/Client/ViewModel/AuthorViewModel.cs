using System.Collections.Generic;
using System.Linq;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Client.ViewModel
{
    public class AuthorViewModel : ViewModel
    {
        private IAuthor _author;
        public IAuthor Author
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

        public AuthorViewModel(IAuthor author)
        {
            Author = author;
        }
    }
}
