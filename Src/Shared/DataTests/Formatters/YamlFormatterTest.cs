using System;
using System.Collections.Generic;
using TPUM.Shared.Data.Formatters;
using Xunit;
using YamlDotNet.Core;

namespace TPUM.Shared.DataTests.Formatters
{
    public class YamlFormatterTest
    {
        public static IEnumerable<object[]> GetTestKnownTypes()
        {
            yield return new[] { new[] { typeof(TestEntity) }, new[] { typeof(TestEntity) } };
            yield return new[] { new[] { typeof(TestEntity) }, new[] { typeof(TestEntity), typeof(TestEntity) } };
            yield return new[] { new[] { typeof(TestEntity), typeof(DerivedTestEntity) }, new[] { typeof(TestEntity), typeof(DerivedTestEntity) } };
            yield return new[] { new[] { typeof(TestEntity) }, null };
        }

        public static string BaseTag => typeof(TestEntity).FullName;
        public static string DerivedTag => typeof(DerivedTestEntity).FullName;

        [Fact]
        public void ParameterlessConstructorTest()
        {
            YamlFormatter<TestEntity> yamlFormatter = new();
            Assert.Equal(new[] { typeof(TestEntity) }, yamlFormatter.KnownTypes);
        }

        [Theory]
        [MemberData(nameof(GetTestKnownTypes))]
        public void ParameterfulConstructorTest(Type[] expected, Type[] types)
        {
            YamlFormatter<TestEntity> yamlFormatter = new(types);
            Assert.Equal(expected, yamlFormatter.KnownTypes);
        }

        [Fact]
        public void FormatTest_BaseEntity()
        {
            TestEntity entity = new()
            {
                Id = 1,
                Foo = "BarBar"
            };
            YamlFormatter<TestEntity> sut = new();
            string yaml = sut.FormatObject(entity);
            Assert.NotNull(yaml);
            Assert.NotEmpty(yaml);
            Assert.Equal(@$"!{BaseTag}
Id: 1
Foo: BarBar
", yaml);
        }

        [Fact]
        public void DeformatTest_BaseEntity()
        {
            string yaml = @$"!{BaseTag}
Id: 1
Foo: Bar
";
            YamlFormatter<TestEntity> sut = new();
            TestEntity result = sut.Deformat(yaml);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Bar", result.Foo);
        }

        [Fact]
        public void FormatTest_DerivedEntity()
        {
            DerivedTestEntity entity = new()
            {
                Id = 1,
                Foo = "BarBar",
                Count = 3
            };
            YamlFormatter<TestEntity> sut_base = new();
            Assert.Throws<YamlException>(() => sut_base.FormatObject(entity));
            YamlFormatter<TestEntity> sut_base2 = new(new[] { typeof(TestEntity), typeof(DerivedTestEntity) });
            string yaml = sut_base2.FormatObject(entity);
            Assert.NotNull(yaml);
            Assert.NotEmpty(yaml);
            Assert.Equal(@$"!{DerivedTag}
Count: 3
Id: 1
Foo: BarBar
", yaml);
            YamlFormatter<DerivedTestEntity> sut_derived = new();
            yaml = sut_derived.FormatObject(entity);
            Assert.NotNull(yaml);
            Assert.NotEmpty(yaml);
            Assert.Equal(@$"!{DerivedTag}
Count: 3
Id: 1
Foo: BarBar
", yaml);
            YamlFormatter<DerivedTestEntity> sut_derived2 = new(new[] { typeof(DerivedTestEntity) });
            yaml = sut_derived2.FormatObject(entity);
            Assert.NotNull(yaml);
            Assert.NotEmpty(yaml);
            Assert.Equal(@$"!{DerivedTag}
Count: 3
Id: 1
Foo: BarBar
", yaml);
            YamlFormatter<TestEntity> sut_derived3 = new(new[] { typeof(DerivedTestEntity) });
            yaml = sut_derived3.FormatObject(entity);
            Assert.NotNull(yaml);
            Assert.NotEmpty(yaml);
            Assert.Equal(@$"!{DerivedTag}
Count: 3
Id: 1
Foo: BarBar
", yaml);
        }

        [Fact]
        public void DeformatTest_DerivedEntity()
        {
            string yaml = @$"!{DerivedTag}
Count: 3
Id: 1
Foo: BarBar
";
            YamlFormatter<TestEntity> sut_base = new();
            Assert.Throws<YamlException>(() => sut_base.Deformat(yaml));
            YamlFormatter<DerivedTestEntity> sut_derived = new();
            TestEntity result = sut_derived.Deformat(yaml);
            Assert.NotNull(result);
            Assert.Equal("BarBar", result.Foo);
            Assert.IsType<DerivedTestEntity>(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(3, (result as DerivedTestEntity).Count);
            YamlFormatter<TestEntity> sut_derived2 = new(new[] { typeof(DerivedTestEntity) });
            result = sut_derived2.Deformat(yaml);
            Assert.NotNull(result);
            Assert.IsType<DerivedTestEntity>(result);
            Assert.Equal("BarBar", result.Foo);
            Assert.Equal(1, result.Id);
            Assert.Equal(3, (result as DerivedTestEntity).Count);
        }

        [Fact]
        public void DeformatTest_DerivedEntityWithoutProperty()
        {
            string yaml = @$"!{DerivedTag}
Id: 1
Foo: BarBar
";
            YamlFormatter<DerivedTestEntity> sut_derived = new();
            DerivedTestEntity result = sut_derived.Deformat(yaml);
            Assert.NotNull(result);
            Assert.IsType<DerivedTestEntity>(result);
            Assert.Equal("BarBar", result.Foo);
            Assert.Equal(1, result.Id);
            Assert.Equal(0, result.Count);
        }
    }
}
