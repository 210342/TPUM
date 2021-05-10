using System;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Server.Logic.Core
{
    public interface IWebSocketResponseHandler : IDisposable
    {
        void Handle();
        bool SendEntity(IEntity entity, ISerializer<INetworkPacket> serializer, Uri sourceUri);

        event Action OnClosing;
    }
}
