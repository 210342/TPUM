using System;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Server.Logic.Core
{
    public interface IWebSocketResponseHandler : IDisposable
    {
        void Handle();
        bool SendEntity(INetworkPacket packet, Func<INetworkPacket, byte[]> serializer);

        event Action OnClosing;
    }
}
