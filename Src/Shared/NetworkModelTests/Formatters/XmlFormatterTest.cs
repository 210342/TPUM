using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TPUM.Shared.NetworkModel.Formatters;
using Xunit;

namespace TPUM.Shared.NetworkModelTests.Formatters
{
    public class XmlFormatterTest
    {
        public static IEnumerable<object[]> GetTestKnownTypes()
        {
            yield return new[] { new[] { typeof(TestEntity) }, new[] { typeof(TestEntity) } };
            yield return new[] { new[] { typeof(TestEntity) }, new[] { typeof(TestEntity), typeof(TestEntity) } };
            yield return new[] { new[] { typeof(TestEntity), typeof(DerivedTestEntity) }, new[] { typeof(TestEntity), typeof(DerivedTestEntity) } };
            yield return new[] { new[] { typeof(TestEntity) }, null };
        }

        [Fact]
        public void ParameterlessConstructorTest()
        {
            XmlFormatter<TestEntity> xmlFormatter = new();
            Assert.Equal(new[] { typeof(TestEntity) }, xmlFormatter.KnownTypes);
        }

        [Theory]
        [MemberData(nameof(GetTestKnownTypes))]
        public void ParameterfulConstructorTest(Type[] expected, Type[] types)
        {
            XmlFormatter<TestEntity> xmlFormatter = new(types);
            Assert.Equal(expected, xmlFormatter.KnownTypes);
        }

        [Fact]
        public void FormatTest_BaseEntity()
        {
            TestEntity entity = new()
            {
                Id = 1,
                Foo = "Bar"
            };
            XmlFormatter<TestEntity> sut = new();
            string xml = sut.FormatObject(entity);
            Assert.NotNull(xml);
            Assert.NotEmpty(xml);
            Assert.Equal(@"<TestEntity xmlns=""tpum.example.com"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""><Foo>Bar</Foo><Id>1</Id></TestEntity>", xml);
        }

        [Fact]
        public void DeformatTest_BaseEntity()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?><TestEntity xmlns=""tpum.example.com"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""><Foo>Bar</Foo><Id>1</Id></TestEntity>";
            XmlFormatter<TestEntity> sut = new();
            TestEntity result = sut.Deformat(xml);
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
            XmlFormatter<TestEntity> sut_base = new();
            Assert.Throws<SerializationException>(() => sut_base.FormatObject(entity));
            XmlFormatter<TestEntity> sut_base2 = new(new[] { typeof(TestEntity), typeof(DerivedTestEntity) });
            string xml = sut_base2.FormatObject(entity);
            Assert.NotNull(xml);
            Assert.NotEmpty(xml);
            Assert.Equal(@"<TestEntity i:type=""DerivedTestEntity"" xmlns=""tpum.example.com"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""><Foo>BarBar</Foo><Id>1</Id><Count>3</Count></TestEntity>", xml);
            XmlFormatter<DerivedTestEntity> sut_derived = new();
            xml = sut_derived.FormatObject(entity);
            Assert.NotNull(xml);
            Assert.NotEmpty(xml);
            Assert.Equal(@"<DerivedTestEntity xmlns=""tpum.example.com"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""><Foo>BarBar</Foo><Id>1</Id><Count>3</Count></DerivedTestEntity>", xml);
        }

        [Fact]
        public void DeformatTest_DerivedEntity()
        {
            string baseXml = @"<?xml version=""1.0"" encoding=""utf-8""?><TestEntity i:type=""DerivedTestEntity"" xmlns=""tpum.example.com"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""><Foo>BarBar</Foo><Id>1</Id></TestEntity>";
            string derivedXml = @"<?xml version=""1.0"" encoding=""utf-8""?><DerivedTestEntity xmlns=""tpum.example.com"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""><Foo>BarBar</Foo><Id>1</Id><Count>3</Count></DerivedTestEntity>";
            XmlFormatter<TestEntity> sut_base = new();
            Assert.Throws<SerializationException>(() => sut_base.Deformat(baseXml));
            sut_base = new(new[] { typeof(DerivedTestEntity), typeof(TestEntity) });
            TestEntity result = sut_base.Deformat(baseXml);
            Assert.NotNull(result);
            Assert.Equal("BarBar", result.Foo);
            Assert.IsAssignableFrom<TestEntity>(result);
            Assert.Equal(1, result.Id);
            XmlFormatter<DerivedTestEntity> sut_derived = new();
            result = sut_derived.Deformat(baseXml);
            Assert.NotNull(result);
            Assert.Equal("BarBar", result.Foo);
            Assert.IsAssignableFrom<TestEntity>(result);
            Assert.Equal(1, result.Id);
            XmlFormatter<DerivedTestEntity> sut_derived2 = new();
            result = sut_derived2.Deformat(derivedXml);
            Assert.NotNull(result);
            Assert.Equal("BarBar", result.Foo);
            Assert.IsType<DerivedTestEntity>(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(3, (result as DerivedTestEntity).Count);
            XmlFormatter<DerivedTestEntity> sut_derived3 = new(new[] { typeof(TestEntity) });
            result = sut_derived3.Deformat(baseXml);
            Assert.NotNull(result);
            Assert.IsType<DerivedTestEntity>(result);
            Assert.Equal("BarBar", result.Foo);
            Assert.Equal(1, result.Id);
            Assert.Equal(0, (result as DerivedTestEntity).Count);
        }

        [Fact]
        public void DeformatTest_DerivedEntityWithoutProperty()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?><DerivedTestEntity xmlns=""tpum.example.com"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""><Foo>BarBar</Foo><Id>1</Id></DerivedTestEntity>";
            XmlFormatter<DerivedTestEntity> sut_derived = new();
            DerivedTestEntity result = sut_derived.Deformat(xml);
            Assert.NotNull(result);
            Assert.IsType<DerivedTestEntity>(result);
            Assert.Equal("BarBar", result.Foo);
            Assert.Equal(1, result.Id);
            Assert.Equal(0, result.Count);
        }
    }
}
