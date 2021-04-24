﻿using System.Windows.Controls;
using TPUM.Shared.ViewModel;

namespace TPUM.Shared.View
{
    /// <summary>
    /// Interaction logic for StockControl.xaml
    /// </summary>
    public partial class StockControl : UserControl
    {
        public StockControl()
        {
            InitializeComponent();
            DataContext = new StockViewModel(
                new Dispatcher(Dispatcher)
            );
        }
    }
}
