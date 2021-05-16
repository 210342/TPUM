using System;

namespace TPUM.Shared.Logic.WebModel
{
    public interface INetworkPacket
    {
        Uri Source { get; set; }
        Guid TypeIdentifier { get; set; }
        object Entity { get; set; }

        byte[] Serialize();
    }
}
