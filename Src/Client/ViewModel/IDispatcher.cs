using System;
using System.Threading.Tasks;

namespace TPUM.Client.ViewModel
{
    public interface IDispatcher
    {
        void Invoke(Action action);
        Task InvokeAsync(Action action);
    }
}
