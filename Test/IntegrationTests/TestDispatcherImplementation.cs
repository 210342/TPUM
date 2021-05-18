using System;
using System.Threading.Tasks;
using TPUM.Client.ViewModel;

namespace TPUM.IntegrationTests
{
    internal class TestDispatcherImplementation : IDispatcher
    {
        public void Invoke(Action action)
        {
            action?.Invoke();
        }

        public Task InvokeAsync(Action action)
        {
            return Task.Run(action);
        }
    }
}
