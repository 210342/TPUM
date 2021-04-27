using System.Collections.Generic;
using System.Linq;
using TPUM.Shared.Logic.Dto;

namespace TPUM.Shared.ViewModel
{
    public class BookViewModel : ViewModel
    {
        private IBookDto _book;
        public IBookDto Book
        {
            get => _book;
            set
            {
                _book = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<AuthorViewModel> AuthorVMs => Book?.AuthorDtos.Select(a => new AuthorViewModel(a));

        public BookViewModel()
        {

        }

        public BookViewModel(IBookDto book)
        {
            Book = book;
        }
    }
}
