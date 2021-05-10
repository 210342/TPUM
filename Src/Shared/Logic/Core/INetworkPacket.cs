using System;

namespace TPUM.Shared.Logic.Core
{
    public interface INetworkPacket
    {
        Uri Source { get; set; }
        Guid TypeIdentifier { get; set; }
        object Entity { get; set; }

        byte[] Serialize(ISerializer<INetworkPacket> serializer);
    }
}
