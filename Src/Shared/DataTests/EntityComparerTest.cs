using System;
using System.Collections.Generic;
using TPUM.Shared.Data;
using TPUM.Shared.Data.Entities;
using Xunit;

namespace TPUM.Shared.DataTests
{
    public class EntityComparerTest
    {
        private class TestEntity : Entity { }

        private class DerivedTestEntity : TestEntity { }

        public static IEnumerable<object[]> ExpectedIds()
        {
            yield return new object[] { 0, 1, -1 };
            yield return new object[] { 1, 1, 0 };
            yield return new object[] { 1, -1, 2 };
        }

        public static IEnumerable<object[]> UnexpectedIds()
        {
            yield return new object[] { typeof(TestEntity), typeof(DerivedTestEntity), 0 };
            yield return new object[] { typeof(DerivedTestEntity), typeof(TestEntity), 0 };
            yield return new object[] { typeof(TestEntity), typeof(TestEntity), 1 };
            yield return new object[] { typeof(TestEntity), typeof(TestEntity), -1 };
        }

        public static IEnumerable<object[]> CompareNullParams()
        {
            yield return new object[] { null, null, -1 };
            yield return new object[] { null, new TestEntity(), -1 };
            yield return new object[] { new TestEntity(), null, 1 };
        }

        public static IEnumerable<object[]> EqualsParams()
        {
            yield return new object[] { new TestEntity() { Id = 0 }, new TestEntity() { Id = 0 }, true };
            yield return new object[] { new TestEntity() { Id = 0 }, new TestEntity() { Id = 1 }, false };
            yield return new object[] { new TestEntity() { Id = 1 }, new DerivedTestEntity() { Id = 1 }, false };
            yield return new object[] { new TestEntity(), new DerivedTestEntity() , false };
            yield return new object[] { new DerivedTestEntity(), new TestEntity() , false };
            yield return new object[] { new DerivedTestEntity() { Id = 0 }, new DerivedTestEntity() { Id = 1 } , false };
            yield return new object[] { new DerivedTestEntity() { Id = 1 }, new DerivedTestEntity() { Id = 1 } , true };
        }

        public static IEnumerable<object[]> GetHashCodeParams()
        {
            yield return new object[] { new TestEntity() { Id = 0 }, new TestEntity() { Id = 0 }, true };
            yield return new object[] { new TestEntity() { Id = 0 }, new TestEntity() { Id = 1 }, false };
            yield return new object[] { new TestEntity() { Id = 1 }, new DerivedTestEntity() { Id = 1 }, true };
            yield return new object[] { new TestEntity(), new DerivedTestEntity(), true };
            yield return new object[] { new DerivedTestEntity(), new TestEntity(), true };
            yield return new object[] { new DerivedTestEntity() { Id = 0 }, new DerivedTestEntity() { Id = 1 }, false };
            yield return new object[] { new DerivedTestEntity() { Id = 1 }, new DerivedTestEntity() { Id = 1 }, true };
        }

        [Theory]
        [MemberData(nameof(ExpectedIds))]
        public void CompareTest_Exact(int lhsId, int rhsId, int expectedValue)
        {
            Entity lhs = new TestEntity() { Id = lhsId }; 
            Entity rhs = new TestEntity() { Id = rhsId };
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
