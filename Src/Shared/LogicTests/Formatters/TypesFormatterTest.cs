using System;
using System.Collections.Generic;
using TPUM.Shared.Logic.Formatters;
using Xunit;

namespace TPUM.Shared.LogicTests.Formatters
{
    public class TypesFormatterTest
    {
        public static IEnumerable<object[]> GetTestKnownTypes()
        {
            yield return new[] { new[] { typeof(TestEntity) }, new[] { typeof(TestEntity) } };
            yield return new[] { new[] { typeof(TestEntity), typeof(TestEntity) }, new[] { typeof(TestEntity) } };
            yield return new[] { new Type[] { null }, new[] { typeof(TestEntity) } };
            yield return new[] { null, new[] { typeof(TestEntity) } };
            yield return new[] { Array.Empty<Type>(), new[] { typeof(TestEntity) } };
        }

        [Fact]
        public void DefaultConstructorTest()
        {
            TypesFormatter<TestEntity> sut = new YamlFormatter<TestEntity>();
            Assert.NotNull(sut.KnownTypes);
            Assert.NotEmpty(sut.KnownTypes);
        }

        [Theory]
        [MemberData(nameof(GetTestKnownTypes))]
        public void ConstructorTest(Type[] types, Type[] expected)
        {
            TypesFormatter<TestEntity> sut = new YamlFormatter<TestEntity>(types);
            Assert.NotNull(sut.KnownTypes);
            Assert.NotEmpty(sut.KnownTypes);
            Assert.Equal(expected, sut.KnownTypes);
        }
    }
}
