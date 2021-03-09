using System;
using TPUM.Shared.Model.Core;
using Xunit;

namespace ModelTests.Core
{
    public class EntityTest
    {
        [Fact]
        public void TypeGuidTest()
        {
            Assert.True(typeof(Entity).GUID.Equals(Guid.Parse("ABE70968-E4A3-4349-9CE3-FDCD529F0081")));
        }

        [Fact]

        public void EmptyConstructorTest()
        {
            Entity entity = new Entity();
            Assert.Equal(0, entity.Id);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(-500, -500)]
        [InlineData(int.MaxValue, int.MaxValue)]
        [InlineData(int.MinValue, int.MinValue)]

        public void IdSetterTest(int setId, int expectedId)
        {
            Entity entity = new Entity()
            {
                Id = setId
            };
            Assert.Equal(expectedId, entity.Id);
        }

        [Fact]
        public void EqualsTest()
        {
            Entity entity1 = new Entity { Id = 0 };
            Entity entity2 = new Entity { Id = 1 };
            Entity entity3 = new Entity { Id = 0 };
            Assert.False(entity1.Equals(entity2));
            Assert.True(entity1.Equals(entity3));
            Assert.False(entity1 == entity3);
        }

        [Fact]
        public void GetHashCodeTest()
        {
            Entity entity1 = new Entity { Id = 0 };
            Entity entity2 = new Entity { Id = 1 };
            Entity entity3 = new Entity { Id = 0 };
            Assert.NotEqual(entity1.GetHashCode(), entity2.GetHashCode());
            Assert.Equal(entity1.GetHashCode(), entity3.GetHashCode());
        }
    }
}
