using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TPUM.Shared.Model;
using TPUM.Shared.Model.Core;
using TPUM.Shared.Model.Entities;
using Xunit;

namespace TPUM.Shared.ModelTests
{
    public class RepositoryTest
    {
        [Fact]
        public void ConstructorTest_NullParameter()
        {
            Assert.Throws<ArgumentNullException>(() => new Repository(null));
        }

        [Fact]
        public void ConstructorTest_NotNullParameter()
        {
            Repository sut = new(DataContext.GetExampleContext());
            DataContext context = sut
                .GetType()
                .GetField("_dataContext", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(sut) as DataContext;
            IDisposable subscription = sut
                .GetType()
                .GetField("_dataContextSubscription", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(sut) as IDisposable;
            Assert.NotNull(context);
            Assert.NotNull(subscription);
            Assert.Equal(3, context.Books.Count);
            Assert.Equal(2, context.Authors.Count);
        }

        [Fact]
        public void GetBooksTest()
        {
            Repository sut = new(DataContext.GetExampleContext());
            List<Book> books = sut.GetBooks();
            Assert.Equal(3, books.Count);
        }

        [Fact]
        public void GetAuthorsTest()
        {
            Repository sut = new(DataContext.GetExampleContext());
            List<Author> authors = sut.GetAuthors();
            Assert.Equal(2, authors.Count);
        }

        [Fact]
        public async Task GetBooksAsyncTest()
        {
            Repository sut = new(DataContext.GetExampleContext());
            List<Book> books = await sut.GetBooksAsync();
            Assert.Equal(3, books.Count);
        }

        [Fact]
        public async Task GetAuthorsAsyncTest()
        {
            Repository sut = new(DataContext.GetExampleContext());
            List<Author> authors = await sut.GetAuthorsAsync();
            Assert.Equal(2, authors.Count);
        }

        [Fact]
        public void GetBookByIdTest()
        {
            int id = 4;
            Repository sut = new(DataContext.GetExampleContext());
            Book book = sut.GetBookById(id);
            Assert.NotNull(book);
            Assert.Equal(id, book.Id);
        }

        [Fact]
        public void GetAuthorByIdTest()
        {
            int id = 2;
            Repository sut = new(DataContext.GetExampleContext());
            Author author = sut.GetAuthorById(id);
            Assert.NotNull(author);
            Assert.Equal(id, author.Id);
        }

        [Fact]
        public void UpdateAuthorsTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(DataContext.GetExampleContext()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<Entity>())).Callback(() => ++subscriptionRaisedCount);
            sut.UpdateAuthors(new List<Author>() { new Author() { Id = 6, NickName = "Homer" } });
            Assert.Single(sut.GetAuthors());
            Assert.Equal(1, subscriptionRaisedCount);
        }

        [Fact]
        public void UpdateBooksTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(DataContext.GetExampleContext()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<Entity>())).Callback(() => ++subscriptionRaisedCount);
            sut.UpdateBooks(new List<Book>() { new Book() { Id = 7, Title = "Iliad" } });
            Assert.Single(sut.GetBooks());
            Assert.Equal(1, subscriptionRaisedCount);
        }

        [Fact]
        public void AddEntityNullTest()
        {
            Repository sut = new(new DataContext());
            sut.AddEntity(null);
            Assert.Empty(sut.GetAuthors());
            Assert.Empty(sut.GetBooks());
        }

        [Fact]
        public void AddAuthorNullTest()
        {
            Repository sut = new(new DataContext());
            Author returned = sut.AddAuthor(null);
            Assert.Null(returned);
            Assert.Empty(sut.GetAuthors());
            Assert.Empty(sut.GetBooks());
        }

        [Fact]
        public void AddBookNullTest()
        {
            Repository sut = new(new DataContext());
            Book returned = sut.AddBook(null);
            Assert.Null(returned);
            Assert.Empty(sut.GetAuthors());
            Assert.Empty(sut.GetBooks());
        }

        [Fact]
        public void AddEntityBookTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(new DataContext()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<Entity>())).Callback(() => ++subscriptionRaisedCount);
            sut.AddEntity(new Book() { Id = 7, Title = "Iliad" });
            Assert.Single(sut.GetBooks());
            Assert.Equal(1, subscriptionRaisedCount);
        }

        [Fact]
        public void AddEntityBookWithAuthorsTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(new DataContext()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<Entity>())).Callback(() => ++subscriptionRaisedCount);
            Author author1 = new() { Id = 8, NickName = "A" };
            Author author2 = new() { Id = 9, NickName = "B" };
            Book book = new() { Id = 10, Title = "C", Authors = new List<Author>() { author1, author2 } };
            author1.Books.Add(book);
            author2.Books.Add(book);
            sut.AddEntity(book);
            Assert.Single(sut.GetBooks());
            Assert.Equal(3, subscriptionRaisedCount);
        }

        [Fact]
        public void AddBookTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(new DataContext()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<Entity>())).Callback(() => ++subscriptionRaisedCount);
            sut.AddBook(new Book() { Id = 7, Title = "Iliad" });
            Assert.Single(sut.GetBooks());
            Assert.Equal(1, subscriptionRaisedCount);
        }

        [Fact]
        public void AddBookWithAuthorsTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(new DataContext()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<Entity>())).Callback(() => ++subscriptionRaisedCount);
            Author author1 = new() { Id = 8, NickName = "A" };
            Author author2 = new() { Id = 9, NickName = "B" };
            Book book = new() { Id = 10, Title = "C", Authors = new List<Author>() { author1, author2 } };
            author1.Books.Add(book);
            author2.Books.Add(book);
            sut.AddBook(book);
            Assert.Single(sut.GetBooks());
            Assert.Equal(3, subscriptionRaisedCount);
        }

        [Fact]
        public void AddEntityAuthorTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(new DataContext()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<Entity>())).Callback(() => ++subscriptionRaisedCount);
            sut.AddEntity(new Author() { Id = 11, FirstName = "D" });
            Assert.Single(sut.GetAuthors());
            Assert.Equal(1, subscriptionRaisedCount);
        }

        [Fact]
        public void AddEntityAuthorWithBooksTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(new DataContext()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<Entity>())).Callback(() => ++subscriptionRaisedCount);
            Book book1 = new() { Id = 12, Title = "E" };
            Book book2 = new() { Id = 13, Title = "F" };
            Author author = new() { Id = 14, LastName = "G", Books = new List<Book>() { book1, book2 } };
            book1.Authors.Add(author);
            book1.Authors.Add(author);
            sut.AddEntity(author);
            Assert.Single(sut.GetAuthors());
            Assert.Equal(3, subscriptionRaisedCount);
        }

        [Fact]
        public void AddAuthorTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(new DataContext()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<Entity>())).Callback(() => ++subscriptionRaisedCount);
            sut.AddAuthor(new Author() { Id = 11, FirstName = "D" });
            Assert.Single(sut.GetAuthors());
            Assert.Equal(1, subscriptionRaisedCount);
        }

        [Fact]
        public void AddAuthorWithBooksTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(new DataContext()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<Entity>())).Callback(() => ++subscriptionRaisedCount);
            Book book1 = new() { Id = 12, Title = "E" };
            Book book2 = new() { Id = 13, Title = "F" };
            Author author = new() { Id = 14, LastName = "G", Books = new List<Book>() { book1, book2 } };
            book1.Authors.Add(author);
            book1.Authors.Add(author);
            sut.AddAuthor(author);
            Assert.Single(sut.GetAuthors());
            Assert.Equal(3, subscriptionRaisedCount);
        }

        [Fact]
        public void AddExistingAuthorTest()
        {
            Repository sut = new(DataContext.GetExampleContext());
            Author author = new() { Id = 2 };
            int authorCount = sut.GetAuthors().Count;
            Author returned = sut.AddAuthor(author);
            Assert.Same(author, returned);
            Assert.Equal(authorCount, sut.GetAuthors().Count);
        }

        [Fact]
        public void AddExistingBookTest()
        {
            Repository sut = new(DataContext.GetExampleContext());
            Book book = new() { Id = 1 };
            int bookCount = sut.GetBooks().Count;
            Book returned = sut.AddBook(book);
            Assert.Same(book, returned);
            Assert.Equal(bookCount, sut.GetBooks().Count);
        }

        [Fact]
        public void DisposeTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(new DataContext()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<Entity>())).Callback(() => ++subscriptionRaisedCount);
            sut.AddEntity(new Book() { Id = 15 });
            Assert.Equal(1, subscriptionRaisedCount);
            sut.Dispose();
            sut.GetType()
                .GetField("_dataContext", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(sut, new DataContext());
            sut.AddEntity(new Book() { Id = 16 });
            Assert.Equal(1, subscriptionRaisedCount);
        }

        private static void AddAuthorsInLoop(IRepository repository, Type entityType, int startingId, int count)
        {
            for (int i = startingId; i < startingId + count; ++i)
            {
                Entity entity = Activator.CreateInstance(entityType) as Entity;
                entity.Id = i;
                repository.AddEntity(entity);
            }
        }

        [Theory]
        [InlineData(typeof(Author))]
        [InlineData(typeof(Book))]
        public async Task AddEntityConcurrentTest(Type entityType)
        {
            int entityCount = 2048;
            Repository sut = new(new DataContext());
            Task[] tasks = new Task[]
            {
                Task.Run(() => AddAuthorsInLoop(sut, entityType, 0, entityCount)),
                Task.Run(() => AddAuthorsInLoop(sut, entityType, entityCount, entityCount)),
            };
            await Task.WhenAll(tasks);
            Assert.Equal(
                tasks.Length * entityCount,
                entityType.Name == nameof(Author) ? sut.GetAuthors().Count : sut.GetBooks().Count
            );
        }
    }
}
