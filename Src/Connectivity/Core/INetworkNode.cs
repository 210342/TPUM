using System;
using System.Threading.Tasks;

namespace TPUM.Connectivity.Core
{
    public interface INetworkNode : IDisposable
    {
        Task Start();
        void Stop();
        void Stop(int delay);
    }
}
