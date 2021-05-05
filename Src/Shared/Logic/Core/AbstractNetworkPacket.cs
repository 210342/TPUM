using Newtonsoft.Json;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TPUM.Shared.Logic.WebModel;
using YamlDotNet.Serialization;

namespace TPUM.Shared.Logic.Core
{
    [DataContract]
    public abstract class AbstractNetworkPacket : INetworkPacket
    {
        private const byte ENTITY_ASCII_SEPARATOR = 0x1E;

        [DataMember]
        [YamlMember(typeof(string))]
        public Uri Source { get; set; }
        [DataMember]
        public Guid TypeIdentifier { get; set; }
        [JsonIgnore]
        [YamlIgnore]
        public object Entity { get; set; }

        public byte[] Serialize(ISerializer<INetworkPacket> serializer)
        {
            ISerializer<Entity> entitySerializer = new Serializer<Entity>(serializer.Encoding, serializer.Formatter.Format);
            byte[] entityBytes = entitySerializer.Serialize(Entity as Entity);
            byte[] networkEntityBytes = serializer.Serialize(this);
            return networkEntityBytes.Concat(GetEntitySeparator(serializer)).Concat(entityBytes).ToArray();
        }

        public static INetworkPacket Deserialize(byte[] source, ISerializer<INetworkPacket> serializer)
        {
            ISerializer<Entity> entitySerializer = new Serializer<Entity>(serializer.Encoding, serializer.Formatter.Format);
            (int separatorStartIndex, int separatorEndIndex) = GetEntitySeparatorRange(source, serializer);
            Span<byte> entityBytes = source.AsSpan(separatorEndIndex, source.Length - separatorEndIndex);
            Span<byte> networkEntityBytes = source.AsSpan(0, separatorStartIndex == -1 ? source.Length : separatorStartIndex);
            INetworkPacket entity = serializer.Deserialize(networkEntityBytes.ToArray());
            if (separatorStartIndex != -1)
            {
                entity.Entity = entitySerializer.Deserialize(entityBytes.ToArray());
            }
            return entity;
        }

        private static (int Start, int End) GetEntitySeparatorRange(byte[] source, ISerializer<INetworkPacket> serializer)
        {
            byte[] separator = GetEntitySeparator(serializer);
            int length = source.Length - separator.Length;
            for (int i = 0; i < length; ++i)
            {
                bool found = true;
                for (int j = 0; j < separator.Length && found; ++j)
                {
                    if (source[i + j] != separator[j])
                    {
                        found = false;
                    }
                }
                if (found)
                {
                    return (i, i + separator.Length);
                }
            }
            return (-1, -1);
        }

        private static byte[] GetEntitySeparator(ISerializer<INetworkPacket> serializer)
        {
            return Encoding.Convert(Encoding.ASCII, serializer.Encoding, new[] { ENTITY_ASCII_SEPARATOR });
        }
    }
}
