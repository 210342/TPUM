using System;
using System.Threading.Tasks;
using TPUM.Shared.NetworkModel.Core;

namespace TPUM.Client.Data.Core
{
    public interface ISocket : IWebDataSource, IObservable<IEntity>, IDisposable
    {
        Task Start();
        void Stop();
    }
}
