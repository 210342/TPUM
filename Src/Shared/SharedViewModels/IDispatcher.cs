using System;
using System.Threading.Tasks;

namespace TPUM.Shared.ViewModel
{
    public interface IDispatcher
    {
        void Invoke(Action action);
        Task InvokeAsync(Action action);
    }
}
