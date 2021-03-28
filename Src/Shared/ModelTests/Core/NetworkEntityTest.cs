using System;
using System.Collections.Generic;
using System.Text;
using TPUM.Shared.Model;
using TPUM.Shared.Model.Core;
using Xunit;

namespace TPUM.Shared.ModelTests.Core
{
    public class NetworkEntityTest
    {
        public static IEnumerable<object[]> SerializationParameters()
        {
            yield return new object[] { Format.JSON, Encoding.Unicode };
            yield return new object[] { Format.JSON, Encoding.UTF8 };
            yield return new object[] { Format.JSON, Encoding.UTF32 };
            yield return new object[] { Format.JSON, Encoding.ASCII };
            yield return new object[] { Format.XML, Encoding.Unicode };
            yield return new object[] { Format.XML, Encoding.UTF8 };
            yield return new object[] { Format.XML, Encoding.UTF32 };
            yield return new object[] { Format.XML, Encoding.ASCII };
            yield return new object[] { Format.YAML, Encoding.Unicode };
            yield return new object[] { Format.YAML, Encoding.UTF8 };
            yield return new object[] { Format.YAML, Encoding.UTF32 };
            yield return new object[] { Format.YAML, Encoding.ASCII };
        }

        [Fact]
        public void DefaultConstructorTest()
        {
            NetworkEntity entity = new();
            Assert.Equal(default, entity.TypeIdentifier);
            Assert.Equal(default, entity.Source);
            Assert.Null(entity.Entity);
        }

        [Fact]
        public void TypeIdentifierTest()
        {
            Guid guid = Guid.Parse("21271A5A-5C0F-429D-944C-FBCD9E696E30");
            NetworkEntity entity = new()
            {
                TypeIdentifier = guid
            };
            Assert.Equal(guid, entity.TypeIdentifier);
        }

        [Fact]
        public void SourceTest()
        {
            Uri source = new("https://example.com/foo/bar");
            NetworkEntity entity = new()
            {
                Source = source
            };
            Assert.Equal(source, entity.Source);
        }

        [Fact]
        public void EntityTest()
        {
            Entity entity = new()
            {
                Id = 0
            };
            NetworkEntity networkEntity = new()
            {
                Entity = entity
            };
            Assert.Equal(entity, networkEntity.Entity);
        }

        [Fact]
        public void EqualsTest()
        {
            Uri source1 = new("https://example.com/foo/bar");
            Uri source2 = new("https://example.com/bar/foo");
            Guid typeIdentifier1 = Guid.Parse("21271A5A-5C0F-429D-944C-FBCD9E696E30");
            Guid typeIdentifier2 = Guid.Parse("E69E5FC8-B157-4261-88BC-492558F352C1");
            object entity1 = new Entity() { Id = 0 };
            object entity2 = new Entity() { Id = 1 };
            object entity3 = null;
            NetworkEntity networkEntity111 = new()
            { 
                Source = source1,
                TypeIdentifier = typeIdentifier1,
                Entity = entity1
            };
            NetworkEntity networkEntity112 = new()
            {
                Source = source1,
                TypeIdentifier = typeIdentifier1,
                Entity = entity2
            };
            NetworkEntity networkEntity113 = new()
            {
                Source = source1,
                TypeIdentifier = typeIdentifier1,
                Entity = entity3
            };
            NetworkEntity networkEntity121 = new()
            {
                Source = source1,
                TypeIdentifier = typeIdentifier2,
                Entity = entity1
            };
            NetworkEntity networkEntity211 = new()
            {
                Source = source2,
                TypeIdentifier = typeIdentifier1,
                Entity = entity1
            };
            NetworkEntity networkEntity111_clone = new()
            {
                Source = source1,
                TypeIdentifier = typeIdentifier1,
                Entity = entity1
            };
            Assert.NotNull(networkEntity111);
            Assert.NotNull(networkEntity112);
            Assert.NotNull(networkEntity113);
            Assert.NotNull(networkEntity121);
            Assert.NotNull(networkEntity211);
            Assert.NotNull(networkEntity111_clone);
            Assert.True(networkEntity111.Equals(networkEntity111));
            Assert.True(networkEntity111.Equals(networkEntity111_clone));
            Assert.False(networkEntity111.Equals(networkEntity112));
            Assert.False(networkEntity111.Equals(networkEntity113));
            Assert.False(networkEntity111.Equals(networkEntity121));
            Assert.False(networkEntity111.Equals(networkEntity211));
            Assert.False(networkEntity111.Equals(networkEntity112));
            Assert.False(networkEntity112.Equals(networkEntity113));
            Assert.False(networkEntity112.Equals(networkEntity121));
            Assert.False(networkEntity112.Equals(networkEntity211));
            Assert.False(networkEntity113.Equals(networkEntity121));
            Assert.False(networkEntity113.Equals(networkEntity121));
            Assert.False(networkEntity113.Equals(networkEntity211));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            Uri source1 = new("https://example.com/foo/bar");
            Uri source2 = new("https://example.com/bar/foo");
            Guid typeIdentifier1 = Guid.Parse("21271A5A-5C0F-429D-944C-FBCD9E696E30");
            Guid typeIdentifier2 = Guid.Parse("E69E5FC8-B157-4261-88BC-492558F352C1");
            object entity1 = new Entity() { Id = 0 };
            object entity2 = new Entity() { Id = 1 };
            NetworkEntity networkEntity1 = new()
            {
                Source = source1,
                TypeIdentifier = typeIdentifier1,
                Entity = entity1
            };
            NetworkEntity networkEntity2 = new()
            {
                Source = source1,
                TypeIdentifier = typeIdentifier1,
                Entity = entity2
            };
            Assert.NotEqual(networkEntity1.GetHashCode(), networkEntity2.GetHashCode());
            networkEntity2.Entity = entity1;
            Assert.Equal(networkEntity1.GetHashCode(), networkEntity2.GetHashCode());
            networkEntity2.Source = source2;
            Assert.NotEqual(networkEntity1.GetHashCode(), networkEntity2.GetHashCode());
            networkEntity1.Source = source2;
            Assert.Equal(networkEntity1.GetHashCode(), networkEntity2.GetHashCode());
            networkEntity2.TypeIdentifier = typeIdentifier2;
            Assert.NotEqual(networkEntity1.GetHashCode(), networkEntity2.GetHashCode());
            networkEntity1.TypeIdentifier = typeIdentifier2;
            Assert.Equal(networkEntity1.GetHashCode(), networkEntity2.GetHashCode());
        }

        [Theory]
        [MemberData(nameof(SerializationParameters))]
        public void SerializationTest(Format format, Encoding encoding)
        {
            Uri source = new("https://example.com/foo/bar");
            Guid typeIdentifier = Guid.Parse("21271A5A-5C0F-429D-944C-FBCD9E696E30");
            object entity = new Entity() { Id = 0 };
            NetworkEntity networkEntity = new()
            {
                Source = source,
                TypeIdentifier = typeIdentifier,
                Entity = entity
            };
            Serializer<NetworkEntity> serializer = new(encoding, format);
            byte[] bytes = networkEntity.Serialize(serializer);
            NetworkEntity copy = NetworkEntity.Deserialize(bytes, serializer);
            Assert.Equal(networkEntity, copy);
            Assert.NotSame(networkEntity, copy);
        }
    }
}
