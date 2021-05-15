using System;
using System.Text;
using TPUM.Shared.Logic;
using Xunit;

namespace TPUM.Shared.LogicTests.Entities
{
    public class NetworkPacketTest
    {
        [Fact]
        public void ConstructorTest_NullPacket()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new NetworkPacket(null, null));
            Assert.Equal("packet", exception.ParamName);
        }

        [Fact]
        public void ConstructorTest_NullSerializer()
        {
            NetworkModel.Core.INetworkPacket packet = NetworkModel.Factory.CreateObject<NetworkModel.Core.INetworkPacket>();
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new NetworkPacket(packet, null));
            Assert.Equal("serializer", exception.ParamName);
        }

        [Fact]
        public void ConstructorTest()
        {
            NetworkModel.Core.INetworkPacket packet = NetworkModel.Factory.CreateObject<NetworkModel.Core.INetworkPacket>();
            NetworkModel.Core.ISerializer<NetworkModel.Core.INetworkPacket> serializer = NetworkModel.Factory.CreateSerializer<NetworkModel.Core.INetworkPacket>(NetworkModel.Core.Format.JSON, Encoding.Default);
            NetworkPacket sut = new NetworkPacket(packet, serializer);
            Assert.NotNull(sut);
        }
    }
}
