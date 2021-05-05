using System;
using System.Threading.Tasks;
using TPUM.Client.ViewModel;

namespace TPUM.Client.View
{
    public class Dispatcher : IDispatcher
    {
        private readonly System.Windows.Threading.Dispatcher _dispatcher;

        public Dispatcher(System.Windows.Threading.Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Invoke(Action action)
        {
            _dispatcher?.Invoke(action);
        }

        public Task InvokeAsync(Action action)
        {
            return _dispatcher?.InvokeAsync(action)?.Task;
        }
    }
}
