using System;
using System.Windows.Input;
using TPUM.Shared.Connectivity;
using TPUM.Shared.Model;

namespace TPUM.Shared.ViewModel
{
    public class ConnectionViewModel : ViewModel
    {
        protected readonly IRepository _repository;

        #region Observable properties

        private string _serverAddress = "localhost:5000";
        public string ServerAddress
        {
            get => _serverAddress;
            set
            {
                _serverAddress = value;
                OnPropertyChanged();
            }
        }

        private StockViewModel _stock;
        public StockViewModel Stock
        {
            get => _stock;
            set
            {
                _stock = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConnectCommand { get; protected set; }
        public ICommand DisconnectCommand { get; protected set; }

        #endregion

        public virtual bool CanDisconnect { get; }
        public virtual bool CanConnect => !CanDisconnect && Uri.TryCreate(ServerAddress, UriKind.Absolute, out Uri _);

        public ConnectionViewModel()
        {
            _repository = new Repository(DataContext.ExampleContext());
            Stock = new StockViewModel(_repository);
        }
    }
}
