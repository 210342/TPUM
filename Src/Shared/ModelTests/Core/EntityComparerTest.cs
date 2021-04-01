using System;
using System.Collections.Generic;
using TPUM.Shared.Model.Core;
using Xunit;

namespace TPUM.Shared.ModelTests.Core
{
    public class EntityComparerTest
    {
        private class TestEntity : Entity { }

        public static IEnumerable<object[]> ExpectedIds()
        {
            yield return new object[] { 0, 1, -1 };
            yield return new object[] { 1, 1, 0 };
            yield return new object[] { 1, -1, 2 };
        }

        public static IEnumerable<object[]> UnexpectedIds()
        {
            yield return new object[] { typeof(Entity), typeof(TestEntity), 0 };
            yield return new object[] { typeof(TestEntity), typeof(Entity), 0 };
            yield return new object[] { typeof(Entity), typeof(Entity), 1 };
            yield return new object[] { typeof(Entity), typeof(Entity), -1 };
        }

        public static IEnumerable<object[]> CompareNullParams()
        {
            yield return new object[] { null, null, -1 };
            yield return new object[] { null, new Entity(), -1 };
            yield return new object[] { new Entity(), null, 1 };
        }

        public static IEnumerable<object[]> EqualsParams()
        {
            yield return new object[] { new Entity() { Id = 0 }, new Entity() { Id = 0 }, true };
            yield return new object[] { new Entity() { Id = 0 }, new Entity() { Id = 1 }, false };
            yield return new object[] { new Entity() { Id = 1 }, new TestEntity() { Id = 1 }, false };
            yield return new object[] { new Entity(), new TestEntity() , false };
            yield return new object[] { new TestEntity(), new Entity() , false };
            yield return new object[] { new TestEntity() { Id = 0 }, new TestEntity() { Id = 1 } , false };
            yield return new object[] { new TestEntity() { Id = 1 }, new TestEntity() { Id = 1 } , true };
        }

        public static IEnumerable<object[]> GetHashCodeParams()
        {
            yield return new object[] { new Entity() { Id = 0 }, new Entity() { Id = 0 }, true };
            yield return new object[] { new Entity() { Id = 0 }, new Entity() { Id = 1 }, false };
            yield return new object[] { new Entity() { Id = 1 }, new TestEntity() { Id = 1 }, true };
            yield return new object[] { new Entity(), new TestEntity(), true };
            yield return new object[] { new TestEntity(), new Entity(), true };
            yield return new object[] { new TestEntity() { Id = 0 }, new TestEntity() { Id = 1 }, false };
            yield return new object[] { new TestEntity() { Id = 1 }, new TestEntity() { Id = 1 }, true };
        }

        [Theory]
        [MemberData(nameof(ExpectedIds))]
        public void CompareTest_Exact(int lhsId, int rhsId, int expectedValue)
        {
            Entity lhs = new() { Id = lhsId }; 
            Entity rhs = new() { Id = rhsId };
            Assert.Equal(expectedValue, new EntityComparer().Compare(lhs, rhs));
        }

        [Theory]
        [MemberData(nameof(UnexpectedIds))]
        public void CompareTest_NotExact(Type lhsType, Type rhsType, int unexpectedValue)
        {
            Entity lhs = Activator.CreateInstance(lhsType) as Entity;
            Entity rhs = Activator.CreateInstance(rhsType) as Entity;
            Assert.NotEqual(unexpectedValue, new EntityComparer().Compare(lhs, rhs));
        }

        [Theory]
        [MemberData(nameof(CompareNullParams))]
        public void CompareTest_Nulls(object lhs, object rhs, int expectedValue)
        {
            Assert.Equal(expectedValue, new EntityComparer().Compare(lhs as Entity, rhs as Entity));
        }

        [Theory]
        [MemberData(nameof(EqualsParams))]
        public void EqualsTest(object lhs, object rhs, bool shouldBeEqual)
        {
            if (shouldBeEqual)
            {
                Assert.True(new EntityComparer().Equals(lhs as Entity, rhs as Entity));
            }
            else
            {
                Assert.False(new EntityComparer().Equals(lhs as Entity, rhs as Entity));
            }
        }

        [Theory]
        [MemberData(nameof(GetHashCodeParams))]
        public void GetHashCodeTest(object lhs, object rhs, bool shouldBeEqual)
        {
            EntityComparer comparer = new();
            if (shouldBeEqual)
            {
                Assert.Equal(comparer.GetHashCode(lhs as Entity), comparer.GetHashCode(rhs as Entity));
            }
            else
            {
                Assert.NotEqual(comparer.GetHashCode(lhs as Entity), comparer.GetHashCode(rhs as Entity));
            }
        }
    }
}
