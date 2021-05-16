using System;
using TPUM.Shared.NetworkModel.Core;

namespace TPUM.Shared.Logic
{
    internal class NetworkPacket : WebModel.INetworkPacket
    {
        private readonly INetworkPacket _networkPacket;
        private readonly ISerializer<INetworkPacket> _serializer;

        public Uri Source { get => _networkPacket.Source; set => _networkPacket.Source = value; }
        public Guid TypeIdentifier { get => _networkPacket.TypeIdentifier; set => _networkPacket.TypeIdentifier = value; }
        public object Entity { get => _networkPacket.Entity; set => _networkPacket.Entity = value; }

        internal NetworkPacket(INetworkPacket packet, ISerializer<INetworkPacket> serializer)
        {
            _networkPacket = packet ?? throw new ArgumentNullException(nameof(packet));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public byte[] Serialize()
        {
            return _networkPacket.Serialize(_serializer);
        }
    }
}
