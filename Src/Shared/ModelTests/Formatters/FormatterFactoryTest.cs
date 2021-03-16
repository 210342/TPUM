using System;
using System.Collections.Generic;
using TPUM.Shared.Model.Core;
using TPUM.Shared.Model.Formatters;
using Xunit;

namespace TPUM.Shared.ModelTests.Formatters
{
    public class FormatterFactoryTest
    {
        public static IEnumerable<object[]> GetTestTypes()
        {
            yield return new object[] {Format.JSON, typeof(JsonFormatter<TestEntity>) };
            yield return new object[] {Format.YAML, typeof(YamlFormatter<TestEntity>) };
            yield return new object[] {Format.XML, typeof(XmlFormatter<TestEntity>) };
        }
        public static IEnumerable<object[]> GetTestTypesWithKnownTypes()
        {
            yield return new object[] { Format.JSON, typeof(JsonFormatter<TestEntity>), new[] { typeof(TestEntity) }, new[] { typeof(TestEntity)} };
            yield return new object[] { Format.JSON, typeof(JsonFormatter<TestEntity>), new[] { typeof(DerivedTestEntity) }, new[] { typeof(DerivedTestEntity)} };
            yield return new object[] { Format.YAML, typeof(YamlFormatter<TestEntity>), new[] { typeof(TestEntity) }, new[] { typeof(TestEntity) } };
            yield return new object[] { Format.YAML, typeof(YamlFormatter<TestEntity>), new[] { typeof(DerivedTestEntity) }, new[] { typeof(DerivedTestEntity) } };
            yield return new object[] { Format.XML, typeof(XmlFormatter<TestEntity>), new[] { typeof(TestEntity) }, new[] { typeof(TestEntity)} };
            yield return new object[] { Format.XML, typeof(XmlFormatter<TestEntity>), new[] { typeof(DerivedTestEntity) }, new[] { typeof(DerivedTestEntity) } };
        }

        [Theory]
        [MemberData(nameof(GetTestTypes))]
        public void CreateFormattersTest_DefaultTypes(Format format, Type expectedType)
        {
            IFormatter<TestEntity> sut = FormatterFactory.CreateFormatter<TestEntity>(format);
            Assert.IsType(expectedType, sut);
        }

        [Theory]
        [MemberData(nameof(GetTestTypesWithKnownTypes))]
        public void CreateFormattersTest_CustomTypes(Format format, Type expectedType, IEnumerable<Type> types, IEnumerable<Type> expectedTypes)
        {
            IFormatter<TestEntity> sut = FormatterFactory.CreateFormatter<TestEntity>(format, types);
            Assert.IsType(expectedType, sut);
            if (sut is TypesFormatter<TestEntity> typesFormatter)
            {
                Assert.Equal(expectedTypes, typesFormatter.KnownTypes);
            }
        }
    }
}
