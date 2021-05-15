using System.Collections.Generic;
using TPUM.Server.Data.Entities;
using Xunit;

namespace TPUM.Server.DataTests.Entities
{
    public class AuthorTest
    {
        [Fact]
        public void DefaultConstructorTest()
        {
            Author author = new();
            Assert.Equal(0, author.Id);
            Assert.Null(author.FirstName);
            Assert.Null(author.LastName);
            Assert.Null(author.NickName);
            Assert.Empty(author.Books);
        }

        [Fact]
        public void FirstNameTest()
        {
            string name = "First name";
            Author author = new()
            {
                FirstName = name
            };
            Assert.Equal(name, author.FirstName);
        }

        [Fact]
        public void LastNameTest()
        {
            string name = "First name";
            Author author = new()
            {
                LastName = name
            };
            Assert.Equal(name, author.LastName);
        }

        [Fact]
        public void NickNameTest()
        {
            string name = "First name";
            Author author = new()
            {
                NickName = name
            };
            Assert.Equal(name, author.NickName);
        }

        [Fact]
        public void BooksTest()
        {
            List<IBook> books = new() { new Book() };
            Author author = new()
            {
                Books = books
            };
            Assert.Equal(books, author.Books);
        }

        [Fact]
        public void EqualsTest()
        {
            string name1 = "Name1";
            string name2 = "Name2";
            List<IBook> books1 = new() { new Book() { Id = 1 } };
            List<IBook> books2 = new() { new Book() { Id = 2 } };
            Author author11 = new()
            {
                Id = 1,
                FirstName = name1,
                LastName = name1,
                NickName = string.Empty,
                Books = books1
            };
            Author author11_clone = new()
            {
                Id = 1,
                FirstName = name1,
                LastName = name1,
                NickName = string.Empty,
                Books = books1
            };
            Author author12 = new()
            {
                Id = 2,
                FirstName = name1,
                LastName = name2,
                NickName = string.Empty,
                Books = books1
            };
            Author author111 = new()
            {
                Id = 3,
                FirstName = name1,
                LastName = name1,
                NickName = name1,
                Books = new()
            };
            Author author222 = new()
            {
                Id = 4,
                FirstName = name2,
                LastName = name2,
                NickName = name2,
                Books = new()
            };
            Author author11b1 = new()
            {
                Id = 5,
                FirstName = name1,
                LastName = name1,
                NickName = string.Empty,
                Books = books1
            };
            Author author11b2 = new()
            {
                Id = 6,
                FirstName = name1,
                LastName = name1,
                NickName = string.Empty,
                Books = books2
            };
            Assert.NotNull(author11);
            Assert.NotNull(author11_clone);
            Assert.NotNull(author12);
            Assert.NotNull(author111);
            Assert.NotNull(author222);
            Assert.NotNull(author11b1);
            Assert.NotNull(author11b2);
            Assert.True(author11.Equals(author11));
            Assert.True(author11.Equals(author11_clone));
            Assert.True(author11_clone.Equals(author11));
            Assert.False(author11.Equals(author12));
            Assert.False(author12.Equals(author11));
            Assert.False(author11.Equals(author111));
            Assert.False(author11.Equals(author222));
            Assert.False(author11.Equals(author11b1));
            Assert.False(author11.Equals(author11b2));
            Assert.False(author12.Equals(author111));
            Assert.False(author12.Equals(author222));
            Assert.False(author12.Equals(author11b1));
            Assert.False(author12.Equals(author11b2));
            Assert.False(author11b1.Equals(author11b2));
            Assert.False(author111.Equals(author222));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            string name1 = "Name1";
            string name2 = "Name2";
            List<IBook> books1 = new() { new Book() { Id = 1 } };
            List<IBook> books2 = new() { new Book() { Id = 2 } };
            Author author1 = new()
            {
                Id = 1,
                FirstName = name1,
                LastName = name1,
                NickName = string.Empty,
                Books = books1
            };
            Author author2 = new()
            {
                Id = 2,
                FirstName = name1,
                LastName = name1,
                NickName = string.Empty,
                Books = books1
            };
            Assert.NotEqual(author1.GetHashCode(), author2.GetHashCode());
            author2.Id = 1;
            Assert.Equal(author1.GetHashCode(), author2.GetHashCode());
            author2.FirstName = name2;
            Assert.NotEqual(author1.GetHashCode(), author2.GetHashCode());
            author1.FirstName = name2;
            Assert.Equal(author1.GetHashCode(), author2.GetHashCode());
            author2.LastName = name2;
            Assert.NotEqual(author1.GetHashCode(), author2.GetHashCode());
            author1.LastName = name2;
            Assert.Equal(author1.GetHashCode(), author2.GetHashCode());
            author2.NickName = name1;
            Assert.NotEqual(author1.GetHashCode(), author2.GetHashCode());
            author1.NickName = name1;
            Assert.Equal(author1.GetHashCode(), author2.GetHashCode());
            author2.Books = books2;
            Assert.NotEqual(author1.GetHashCode(), author2.GetHashCode());
            author1.Books = books2;
            Assert.Equal(author1.GetHashCode(), author2.GetHashCode());
            author2.Id = 2;
            Assert.NotEqual(author1.GetHashCode(), author2.GetHashCode());
        }
    }
}
