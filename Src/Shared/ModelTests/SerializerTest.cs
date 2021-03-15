using Moq;
using System;
using System.Text;
using TPUM.Shared.Model.Formatters;
using TPUM.Shared.Model;
using TPUM.Shared.Model.Core;
using Xunit;

namespace TPUM.Shared.ModelTests
{
    public class SerializerTest
    {
        [Fact]
        public void ConstructorTest_NullEncoding_ShouldThrow()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new Serializer<TestEntity>(null, null));
            Assert.Equal("encoding", exception.ParamName);
        }

        [Fact]
        public void ConstructorTest_NullFormatter_ShouldThrow()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new Serializer<TestEntity>(Encoding.Default, null));
            Assert.Equal("formatter", exception.ParamName);
        }

        [Fact]
        public void SerializeTest_ShouldThrow()
        {
            ISerializer<TestEntity> sut = new Serializer<TestEntity>(Encoding.Default, new JsonFormatter<TestEntity>());
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => sut.Serialize(null));
            Assert.Equal("obj", exception.ParamName);
        }

        [Fact]
        public void SerializeTest()
        {
            TestEntity entity = new() { Id = 2, Foo = "Bar" };
            string toEncode = "test mock";
            Mock<IFormatter<TestEntity>> formatterMock = new Mock<IFormatter<TestEntity>>();
            formatterMock.Setup(formatter => formatter.FormatObject(entity)).Returns(toEncode);
            ISerializer<TestEntity> sut = new Serializer<TestEntity>(Encoding.Default, formatterMock.Object);
            byte[] output = sut.Serialize(entity);
            Assert.NotNull(output);
            Assert.NotEmpty(output);
            Assert.Equal(Encoding.Default.GetBytes(toEncode), output);
            Assert.Equal(toEncode, Encoding.Default.GetString(output));
        }

        [Fact]
        public void DeserializeTest_ShouldThrowOnNull()
        {
            ISerializer<TestEntity> sut = new Serializer<TestEntity>(Encoding.Default, new JsonFormatter<TestEntity>());
            ArgumentException exception = Assert.Throws<ArgumentException>(() => sut.Deserialize(null));
            Assert.Equal("bytes", exception.ParamName);
        }

        [Fact]
        public void DeserializeTest_ShouldThrowOnEmpty()
        {
            ISerializer<TestEntity> sut = new Serializer<TestEntity>(Encoding.Default, new JsonFormatter<TestEntity>());
            ArgumentException exception = Assert.Throws<ArgumentException>(() => sut.Deserialize(Array.Empty<byte>()));
            Assert.Equal("bytes", exception.ParamName);
        }

        [Fact]
        public void DeserializeTest()
        {
            TestEntity entity = new() { Id = 2, Foo = "Bar" };
            string toEncode = "test mock";
            byte[] bytes = Encoding.Default.GetBytes(toEncode);
            Mock<IFormatter<TestEntity>> formatterMock = new Mock<IFormatter<TestEntity>>();
            formatterMock.Setup(formatter => formatter.Deformat(toEncode)).Returns(entity);
            ISerializer<TestEntity> sut = new Serializer<TestEntity>(Encoding.Default, formatterMock.Object);
            TestEntity output = sut.Deserialize(bytes);
            Assert.NotNull(output);
            Assert.Equal(entity.Id, output.Id);
            Assert.Equal(entity.Foo, output.Foo);
        }

    }
}
