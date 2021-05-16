using TPUM.Shared.NetworkModel.Formatters;
using Xunit;

namespace TPUM.Shared.NetworkModelTests.Formatters
{
    public class JsonFormatterTest
    {
        [Fact]
        public void FormatTest_BaseEntity()
        {
            TestEntity entity = new()
            {
                Id = 1,
                Foo = "BarBar"
            };
            JsonFormatter<TestEntity> sut = new();
            string json = sut.FormatObject(entity);
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            Assert.Equal("{\"$id\":\"1\",\"$type\":\"TPUM.Shared.NetworkModelTests.TestEntity, NetworkModelTests\",\"Id\":1,\"Foo\":\"BarBar\"}", json);
        }

        [Theory]
        [InlineData("{\"$id\":\"1\",\"Id\":0,\"Foo\":\"Bar\"}")]
        [InlineData("{\"$id\":\"1\",\"id\":0,\"foo\":\"Bar\"}")]
        [InlineData("{\"$id\":\"1\",\"Id\": 0, \"Foo\": \"Bar\"}")]
        [InlineData(@"{
    ""$id"":""1"",
    ""Id"": 0,
    ""Foo"": ""Bar""
}")]
        public void DeformatTest_BaseEntity(string json)
        {
            JsonFormatter<TestEntity> sut = new();
            TestEntity result = sut.Deformat(json);
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
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
            JsonFormatter<TestEntity> sut_base = new();
            string json = sut_base.FormatObject(entity);
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            Assert.Equal("{\"$id\":\"1\",\"$type\":\"TPUM.Shared.NetworkModelTests.DerivedTestEntity, NetworkModelTests\",\"Count\":3,\"Id\":1,\"Foo\":\"BarBar\"}", json);
            JsonFormatter<DerivedTestEntity> sut_derived = new();
            json = sut_derived.FormatObject(entity);
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            Assert.Equal("{\"$id\":\"1\",\"$type\":\"TPUM.Shared.NetworkModelTests.DerivedTestEntity, NetworkModelTests\",\"Count\":3,\"Id\":1,\"Foo\":\"BarBar\"}", json);
        }

        [Theory]
        [InlineData("{\"$id\":\"1\",\"$type\":\"TPUM.Shared.NetworkModelTests.DerivedTestEntity, NetworkModelTests\",\"Id\":0,\"Foo\":\"Bar\",\"Count\":3}")]
        [InlineData("{\"$id\":\"1\",\"$type\":\"TPUM.Shared.NetworkModelTests.DerivedTestEntity, NetworkModelTests\",\"id\":0,\"foo\":\"Bar\",\"counT\":3}")]
        [InlineData("{\"$id\":\"1\",\"$type\":\"TPUM.Shared.NetworkModelTests.DerivedTestEntity, NetworkModelTests\",\"Id\": 0, \"Foo\": \"Bar\", \"Count\": 3}")]
        [InlineData(@"{
    ""$id"":""1"",
    ""$type"":""TPUM.Shared.NetworkModelTests.DerivedTestEntity, NetworkModelTests"",
    ""Id"": 0,
    ""Foo"": ""Bar"",
    ""Count"": 3
}")]
        public void DeFormatTest_DerivedEntity(string json)
        {
            JsonFormatter<TestEntity> sut_base = new();
            TestEntity result = sut_base.Deformat(json);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Foo);
            Assert.Equal(0, result.Id);
            JsonFormatter<DerivedTestEntity> sut_derived = new();
            result = sut_derived.Deformat(json);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Foo);
            Assert.Equal(0, result.Id);
            Assert.Equal(3, (result as DerivedTestEntity).Count);
        }

        [Fact]
        public void DeFormatTest_DerivedEntityWithoutProperty()
        {
            string json = "{\"Id\": 0, \"Foo\": \"Bar\"}";
            JsonFormatter<DerivedTestEntity> sut_derived = new();
            DerivedTestEntity result = sut_derived.Deformat(json);
            Assert.NotNull(result);
            Assert.IsType<DerivedTestEntity>(result);
            Assert.Equal("Bar", result.Foo);
            Assert.Equal(0, result.Id);
            Assert.Equal(0, result.Count);
        }
    }
}
