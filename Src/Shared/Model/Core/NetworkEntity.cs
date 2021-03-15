using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace TPUM.Shared.Model.Core
{
    [DataContract]
    public class NetworkEntity : IExtensibleDataObject
    {
        private const byte ENTITY_ASCII_SEPARATOR = 0x1E;

        #region Data properties
        [DataMember]
        public Uri Source { get; set; }
        [DataMember]
        public Guid TypeIdentifier { get; set; }
        [JsonIgnore]
        public object Entity { get; set; }

        #endregion

        #region Logic properties

        [JsonIgnore]
        public ExtensionDataObject ExtensionData { get; set; }

        #endregion

        public byte[] Serialize(ISerializer<NetworkEntity> serializer)
        {
            ISerializer<Entity> entitySerializer = new Serializer<Entity>(serializer.Encoding, serializer.Formatter.Format);
            byte[] entityBytes = entitySerializer.Serialize(Entity as Entity);
            byte[] networkEntityBytes = serializer.Serialize(this);
            return networkEntityBytes.Concat(GetEntitySeparator(serializer)).Concat(entityBytes).ToArray();
        }

        public static NetworkEntity Deserialize(byte[] source, ISerializer<NetworkEntity> serializer)
        {
            ISerializer<Entity> entitySerializer = new Serializer<Entity>(serializer.Encoding, serializer.Formatter.Format);
            (int Start, int End) entitySeparatorRange = GetEntitySeparatorRange(source, serializer);
            Span<byte> entityBytes = source.AsSpan(entitySeparatorRange.End, source.Length - entitySeparatorRange.End);
            Span<byte> networkEntityBytes = source.AsSpan(0, entitySeparatorRange.Start == -1 ? source.Length : entitySeparatorRange.Start);
            NetworkEntity entity = serializer.Deserialize(networkEntityBytes.ToArray());
            if (entitySeparatorRange.Start != -1)
            {
                entity.Entity = entitySerializer.Deserialize(entityBytes.ToArray());
            }
            return entity;
        }

        public override bool Equals(object obj)
        {
            return obj is NetworkEntity entity &&
                   EqualityComparer<Uri>.Default.Equals(Source, entity.Source) &&
                   TypeIdentifier.Equals(entity.TypeIdentifier) &&
                   EqualityComparer<object>.Default.Equals(Entity, entity.Entity);
        }

        public override int GetHashCode()
        {
            int hashCode = 2103276648;
            hashCode = hashCode * -1521134295 + EqualityComparer<Uri>.Default.GetHashCode(Source);
            hashCode = hashCode * -1521134295 + TypeIdentifier.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Entity);
            return hashCode;
        }

        private static (int Start, int End) GetEntitySeparatorRange(byte[] source, ISerializer<NetworkEntity> serializer)
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

        private static byte[] GetEntitySeparator(ISerializer<NetworkEntity> serializer)
        {
            return Encoding.Convert(Encoding.ASCII, serializer.Encoding, new[] { ENTITY_ASCII_SEPARATOR });
        }
    }
}
