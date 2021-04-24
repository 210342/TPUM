using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TPUM.Shared.ViewModel
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        protected readonly IDispatcher _dispatcher;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ViewModel() : this(null) { }

        public ViewModel(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
    }
}
