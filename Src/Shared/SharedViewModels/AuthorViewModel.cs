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

        public AuthorViewModel()
        {

        }

        public AuthorViewModel(Author author)
        {
            Author = author;
        }
    }
}
