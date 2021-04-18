using System.Windows;
using TPUM.Shared.Model;
using TPUM.Shared.View;
using TPUM.Shared.ViewModel;

namespace TPUM.Server.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new StockViewModel(
                new Repository(), 
                new Dispatcher(Dispatcher)
            );
        }
    }
}
