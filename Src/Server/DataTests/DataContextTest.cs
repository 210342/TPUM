using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using TPUM.Server.Data;
using TPUM.Server.Data.Entities;
using Xunit;

namespace TPUM.Server.DataTests
{
    public class DataContextTest
    {
        private static DataContext CallPrivateConstructor(params object[] args)
        {
            return Activator.CreateInstance(
                typeof(DataContext),
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                args,
                CultureInfo.InvariantCulture,
                Array.Empty<object>()
            ) as DataContext;
        }

        [Fact]
        public void DefaultConstructorTest()
        {
            DataContext sut = new();
            Assert.NotNull(sut.Authors);
            Assert.NotNull(typeof(ObservableCollection<Author>)
                .GetField("CollectionChanged", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(sut.Authors));
            Assert.NotNull(sut.Books);
            Assert.NotNull(typeof(ObservableCollection<Book>)
                .GetField("CollectionChanged", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(sut.Books));
        }

        [Fact]
        public void ConstructorWithParametersTest_Nulls()
        {
            DataContext sut = CallPrivateConstructor(null, null);
            Assert.NotNull(sut.Authors);
            Assert.NotNull(sut.Books);
            Assert.Empty(sut.Authors);
            Assert.Empty(sut.Books);
            Assert.NotNull(typeof(ObservableCollection<Author>)
                .GetField("CollectionChanged", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(sut.Authors));
            Assert.NotNull(typeof(ObservableCollection<Book>)
                .GetField("CollectionChanged", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(sut.Books));
        }

        [Fact]
        public void ConstructorWithParametersTest_Empty()
        {
            DataContext sut = CallPrivateConstructor(new List<Author>(), new List<Book>());
            Assert.NotNull(sut.Authors);
            Assert.NotNull(sut.Books);
            Assert.Empty(sut.Authors);
            Assert.Empty(sut.Books);
            Assert.NotNull(typeof(ObservableCollection<Author>)
                .GetField("CollectionChanged", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(sut.Authors));
            Assert.NotNull(typeof(ObservableCollection<Book>)
                .GetField("CollectionChanged", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(sut.Books));
        }

        [Fact]
        public void ConstructorWithParametersTest_NotEmpty()
        {
            Book book1 = new()
            {
                Title = "title1",
            };
            Book book2 = new()
            {
                Title = "title2",
            };
            List<IBook> books1 = new() { book1, book2 };
            Author author1 = new()
            {
                FirstName = "a",
                LastName = "b",
                NickName = "c",
                Books = books1,
            };
            book1.Authors.Add(author1);
            book2.Authors.Add(author1);
            List<Author> authors = new() { author1 };
            List<Book> books = new() { book1, book2 };
            DataContext sut = CallPrivateConstructor(authors, books);
            Assert.NotNull(sut.Authors);
            Assert.NotNull(sut.Books);
            Assert.NotEmpty(sut.Authors);
            Assert.NotEmpty(sut.Books);
            Assert.NotNull(typeof(ObservableCollection<Author>)
                .GetField("CollectionChanged", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(sut.Authors));
            Assert.NotNull(typeof(ObservableCollection<Book>)
                .GetField("CollectionChanged", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(sut.Books));
            Assert.Equal(authors, sut.Authors);
            Assert.Equal(books, sut.Books);
        }

        [Fact]
        public void DisposeTest()
        {
            Book book1 = new()
            {
                Title = "title1",
            };
            Book book2 = new()
            {
                Title = "title2",
            };
            Author author1 = new()
            {
                FirstName = "a",
                LastName = "b",
                NickName = "c",
                Books = new List<IBook>() { book1, book2 },
            };
            book1.Authors.Add(author1);
            book2.Authors.Add(author1);
            List<Author> authors = new() { author1 };
            List<Book> books = new() { book1, book2 };
            DataContext sut = CallPrivateConstructor(authors, books);
            sut.Dispose();
            Assert.Empty(sut.Authors);
            Assert.Empty(sut.Books);
            Assert.Null(typeof(ObservableCollection<Author>)
                .GetField("CollectionChanged", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(sut.Authors));
            Assert.Null(typeof(ObservableCollection<Book>)
                .GetField("CollectionChanged", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(sut.Books));
        }
    }
}
