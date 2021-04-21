using System;
using System.Collections.Generic;
using System.Text;
using TPUM.Shared.Data;
using TPUM.Shared.Data.Core;
using TPUM.Shared.Data.Entities;
using Xunit;

namespace TPUM.Shared.DataTests.Entities
{
    public class BookTest
    {
        [Fact]
        public void TypeGuidTest()
        {
            Assert.Equal(Guid.Parse("B093A245-0E2D-4E82-A862-4D5567E0F84E"), typeof(Book).GUID);
        }

        [Fact]
        public void DefaultConstructorTest()
        {
            Book book = new();
            Assert.Equal(0, book.Id);
            Assert.Null(book.Title);
            Assert.Empty(book.Authors);
        }

        [Fact]
        public void TitleTest()
        {
            string title = "Title";
            Book book = new()
            {
                Title = title
            };
            Assert.Equal(title, book.Title);
        }

        [Fact]
        public void BooksTest()
        {
            List<IAuthor> authors = new() { new Author() };
            Book book = new()
            {
                Authors = authors
            };
            Assert.Equal(authors, book.Authors);
        }

        [Fact]
        public void EqualsTest()
        {
            string title1 = "Title 1";
            string title2 = "Title 2";
            List<IAuthor> authors1 = new() { new Author() { Id = 1 } };
            List<IAuthor> authors2 = new() { new Author() { Id = 2 } };
            Book book11 = new()
            {
                Id = 1,
                Title = title1,
                Authors = authors1
            };
            Book book11_clone = new()
            {
                Id = 1,
                Title = title1,
                Authors = authors1
            };
            Book book12 = new()
            {
                Id = 2,
                Title = title1,
                Authors = authors2
            };
            Book book21 = new()
            {
                Id = 3,
                Title = title2,
                Authors = authors1
            };
            Book book22 = new()
            {
                Id = 4,
                Title = title2,
                Authors = authors2
            };
            Assert.NotNull(book11);
            Assert.NotNull(book11_clone);
            Assert.NotNull(book12);
            Assert.NotNull(book21);
            Assert.NotNull(book22);
            Assert.True(book11.Equals(book11));
            Assert.True(book11.Equals(book11_clone));
            Assert.True(book11_clone.Equals(book11));
            Assert.False(book11.Equals(book12));
            Assert.False(book11.Equals(book21));
            Assert.False(book11.Equals(book22));
            Assert.False(book12.Equals(book21));
            Assert.False(book12.Equals(book22));
            Assert.False(book21.Equals(book22));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            string title1 = "Title 1";
            string title2 = "Title 2";
            List<IAuthor> authors1 = new() { new Author() { Id = 1 } };
            List<IAuthor> authors2 = new() { new Author() { Id = 2 } };
            Book book1 = new()
            {
                Id = 1,
                Title = title1,
                Authors = authors1
            };
            Book book2 = new()
            {
                Id = 2,
                Title = title1,
                Authors = authors1
            };
            Assert.NotEqual(book1.GetHashCode(), book2.GetHashCode());
            book2.Id = 1;
            Assert.Equal(book1.GetHashCode(), book2.GetHashCode());
            book2.Title = title2;
            Assert.NotEqual(book1.GetHashCode(), book2.GetHashCode());
            book1.Title = title2;
            Assert.Equal(book1.GetHashCode(), book2.GetHashCode());
            book2.Authors = authors2;
            Assert.NotEqual(book1.GetHashCode(), book2.GetHashCode());
            book1.Authors = authors2;
            Assert.Equal(book1.GetHashCode(), book2.GetHashCode());
            book2.Id = 2;
            Assert.NotEqual(book1.GetHashCode(), book2.GetHashCode());
        }

        [Theory]
        [MemberData(nameof(NetworkPacketTest.SerializationParameters), MemberType = typeof(NetworkPacketTest))]
        public void SerializationTest(Format format, Encoding encoding)
        {
            string name1 = "Name1";
            Author author = new()
            {
                Id = 1,
                FirstName = name1,
                LastName = name1,
                NickName = string.Empty
            };
            Book book = new()
            {
                Title = "title1",
                Authors = new List<IAuthor>() { author }
            };
            author.Books.Add(book);
            Serializer<Book> serializer = new(encoding, format, new[] { typeof(Author), typeof(Book) });
            byte[] data = serializer.Serialize(book);
            Book deserialized = serializer.Deserialize(data);
            Assert.Equal(book, deserialized);
        }
    }
}
