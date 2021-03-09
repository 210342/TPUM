using System;
using System.Threading.Tasks;

namespace TPUM.Shared.Connectivity.Core
{
    public interface INetworkNode : IDisposable
    {
        Task Start();
        void Stop();
        void Stop(int delay);
    }
}
