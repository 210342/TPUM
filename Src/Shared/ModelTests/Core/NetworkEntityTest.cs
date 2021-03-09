﻿using System;
using System.Collections.Generic;
using System.Text;
using TPUM.Shared.Model.Core;
using Xunit;

namespace ModelTests.Core
{
    public class NetworkEntityTest
    {
        [Fact]
        public void DefaultConstructorTest()
        {
            NetworkEntity entity = new NetworkEntity();
            Assert.Equal(default, entity.TypeIdentifier);
            Assert.Equal(default, entity.Source);
            Assert.Null(entity.Entity);
        }

        [Fact]
        public void TypeIdentifierTest()
        {
            Guid guid = Guid.Parse("21271A5A-5C0F-429D-944C-FBCD9E696E30");
            NetworkEntity entity = new NetworkEntity()
            {
                TypeIdentifier = guid
            };
            Assert.Equal(guid, entity.TypeIdentifier);
        }

        [Fact]
        public void SourceTest()
        {
            Uri source = new Uri("https://example.com/foo/bar");
            NetworkEntity entity = new NetworkEntity()
            {
                Source = source
            };
            Assert.Equal(source, entity.Source);
        }

        [Fact]
        public void EntityTest()
        {
            Entity entity = new Entity()
            {
                Id = 0
            };
            NetworkEntity networkEntity = new NetworkEntity()
            {
                Entity = entity
            };
            Assert.Equal(entity, networkEntity.Entity);
        }

        [Fact]
        public void EqualsTest()
        {
            Uri source1 = new Uri("https://example.com/foo/bar");
            Uri source2 = new Uri("https://example.com/bar/foo");
            Guid typeIdentifier1 = Guid.Parse("21271A5A-5C0F-429D-944C-FBCD9E696E30");
            Guid typeIdentifier2 = Guid.Parse("E69E5FC8-B157-4261-88BC-492558F352C1");
            object entity1 = new Entity() { Id = 0 };
            object entity2 = new Entity() { Id = 1 };
            object entity3 = null;
            NetworkEntity networkEntity111 = new NetworkEntity 
            { 
                Source = source1,
                TypeIdentifier = typeIdentifier1,
                Entity = entity1
            };
            NetworkEntity networkEntity112 = new NetworkEntity
            {
                Source = source1,
                TypeIdentifier = typeIdentifier1,
                Entity = entity2
            };
            NetworkEntity networkEntity113 = new NetworkEntity
            {
                Source = source1,
                TypeIdentifier = typeIdentifier1,
                Entity = entity3
            };
            NetworkEntity networkEntity121 = new NetworkEntity
            {
                Source = source1,
                TypeIdentifier = typeIdentifier2,
                Entity = entity1
            };
            NetworkEntity networkEntity211 = new NetworkEntity
            {
                Source = source2,
                TypeIdentifier = typeIdentifier1,
                Entity = entity1
            };
            NetworkEntity networkEntity111_clone = new NetworkEntity
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
            Uri source1 = new Uri("https://example.com/foo/bar");
            Uri source2 = new Uri("https://example.com/bar/foo");
            Guid typeIdentifier1 = Guid.Parse("21271A5A-5C0F-429D-944C-FBCD9E696E30");
            Guid typeIdentifier2 = Guid.Parse("E69E5FC8-B157-4261-88BC-492558F352C1");
            object entity1 = new Entity() { Id = 0 };
            object entity2 = new Entity() { Id = 1 };
            NetworkEntity networkEntity1 = new NetworkEntity
            {
                Source = source1,
                TypeIdentifier = typeIdentifier1,
                Entity = entity1
            };
            NetworkEntity networkEntity2 = new NetworkEntity
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
    }
}
