using System;
using System.Threading.Tasks;
using TPUM.Shared.Model.Core;

namespace TPUM.Shared.Connectivity.Core
{
    public interface INetworkNode : IDisposable
    {
        ISerializer<NetworkEntity> Serializer { get; }

        Task Start();
        void Stop();
        void Stop(int delay);
    }
}
