using System.Collections.Generic;
using System.Linq;
using TPUM.Shared.Logic.Dto;

namespace TPUM.Shared.ViewModel
{
    public class AuthorViewModel : ViewModel
    {
        private IAuthorDto _author;
        public IAuthorDto Author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<BookViewModel> BookVMs => Author.BookDtos.Select(b => new BookViewModel(b));

        public AuthorViewModel()
        {

        }

        public AuthorViewModel(IAuthorDto author)
        {
            Author = author;
        }
    }
}
