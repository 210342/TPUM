using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TPUM.Shared.Data.Core;
using TPUM.Shared.Data;
using TPUM.Shared.Data.Entities;
using TPUM.Shared.Logic;
using Xunit;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.Dto;

namespace TPUM.Shared.LogicTests
{
    public class RepositoryTest
    {
        internal class AuthorTestObject : IAuthorDto
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string NickName { get; set; }
            public List<IBook> Books { get; set; } = new List<IBook>();
            public int Id { get; set; }
            public IEnumerable<IBookDto> BookDtos { get; }
            public int EntityId { get; }
        }
        internal class BookTestObject : IBookDto
        {
            public string Title { get; set; }
            public List<IAuthor> Authors { get; set; } = new List<IAuthor>();
            public int Id { get; set; }
            public IEnumerable<IAuthorDto> AuthorDtos { get; }
            public int EntityId { get; }
        }
        [Fact]
        public void ConstructorTest_NullParameter()
        {
            Assert.Throws<ArgumentNullException>(() => new Repository(null));
        }

        [Fact]
        public void ConstructorTest_NotNullParameter()
        {
            Repository sut = new(DataFactory.GetExampleContext());
            IDataContext context = sut
                .GetType()
                .GetField("_dataContext", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(sut) as IDataContext;
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
            Repository sut = new(DataFactory.GetExampleContext());
            List<IBookDto> books = sut.GetBooks();
            Assert.Equal(3, books.Count);
        }

        [Fact]
        public void GetAuthorsTest()
        {
            Repository sut = new(DataFactory.GetExampleContext());
            List<IAuthorDto> authors = sut.GetAuthors();
            Assert.Equal(2, authors.Count);
        }

        [Fact]
        public async Task GetBooksAsyncTest()
        {
            Repository sut = new(DataFactory.GetExampleContext());
            List<IBookDto> books = await sut.GetBooksAsync();
            Assert.Equal(3, books.Count);
        }

        [Fact]
        public async Task GetAuthorsAsyncTest()
        {
            Repository sut = new(DataFactory.GetExampleContext());
            List<IAuthorDto> authors = await sut.GetAuthorsAsync();
            Assert.Equal(2, authors.Count);
        }

        [Fact]
        public void GetBookByIdTest()
        {
            int id = 4;
            Repository sut = new(DataFactory.GetExampleContext());
            IBook book = sut.GetBookById(id);
            Assert.NotNull(book);
            Assert.Equal(id, book.Id);
        }

        [Fact]
        public void GetAuthorByIdTest()
        {
            int id = 2;
            Repository sut = new(DataFactory.GetExampleContext());
            IAuthor author = sut.GetAuthorById(id);
            Assert.NotNull(author);
            Assert.Equal(id, author.Id);
        }

        [Fact]
        public void UpdateAuthorsTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(DataFactory.GetExampleContext()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<IEntity>())).Callback(() => ++subscriptionRaisedCount);
            sut.UpdateAuthors(new List<IAuthorDto>() { new AuthorTestObject() { Id = 6, NickName = "Homer" } });
            Assert.Single(sut.GetAuthors());
            Assert.Equal(1, subscriptionRaisedCount);
        }

        [Fact]
        public void UpdateBooksTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(DataFactory.GetExampleContext()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<IEntity>())).Callback(() => ++subscriptionRaisedCount);
            sut.UpdateBooks(new List<IBookDto>() { new BookTestObject() { Id = 7, Title = "Iliad" } });
            Assert.Single(sut.GetBooks());
            Assert.Equal(1, subscriptionRaisedCount);
        }

        [Fact]
        public void AddEntityNullTest()
        {
            Repository sut = new(DataFactory.CreateObject<IDataContext>());
            sut.AddEntity(null);
            Assert.Empty(sut.GetAuthors());
            Assert.Empty(sut.GetBooks());
        }

        [Fact]
        public void AddAuthorNullTest()
        {
            Repository sut = new(DataFactory.CreateObject<IDataContext>());
            IAuthor returned = sut.AddAuthor(null);
            Assert.Null(returned);
            Assert.Empty(sut.GetAuthors());
            Assert.Empty(sut.GetBooks());
        }

        [Fact]
        public void AddBookNullTest()
        {
            Repository sut = new(DataFactory.CreateObject<IDataContext>());
            IBook returned = sut.AddBook(null);
            Assert.Null(returned);
            Assert.Empty(sut.GetAuthors());
            Assert.Empty(sut.GetBooks());
        }

        [Fact]
        public void AddEntityBookTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(DataFactory.CreateObject<IDataContext>()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<IEntity>())).Callback(() => ++subscriptionRaisedCount);
            sut.AddEntity(new BookTestObject() { Id = 7, Title = "Iliad" });
            Assert.Single(sut.GetBooks());
            Assert.Equal(1, subscriptionRaisedCount);
        }

        [Fact]
        public void AddEntityBookWithAuthorsTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(DataFactory.CreateObject<IDataContext>()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<IEntity>())).Callback(() => ++subscriptionRaisedCount);
            AuthorTestObject author1 = new() { Id = 8, NickName = "A" };
            AuthorTestObject author2 = new() { Id = 9, NickName = "B" };
            BookTestObject book = new() { Id = 10, Title = "C", Authors = new List<IAuthor>() { author1, author2 } };
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
            Mock<Repository> mock = new(() => new Repository(DataFactory.CreateObject<IDataContext>()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<IEntity>())).Callback(() => ++subscriptionRaisedCount);
            sut.AddBook(new BookTestObject() { Id = 7, Title = "Iliad" });
            Assert.Single(sut.GetBooks());
            Assert.Equal(1, subscriptionRaisedCount);
        }

        [Fact]
        public void AddBookWithAuthorsTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(DataFactory.CreateObject<IDataContext>()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<IEntity>())).Callback(() => ++subscriptionRaisedCount);
            AuthorTestObject author1 = new() { Id = 8, NickName = "A" };
            AuthorTestObject author2 = new() { Id = 9, NickName = "B" };
            BookTestObject book = new() { Id = 10, Title = "C", Authors = new List<IAuthor>() { author1, author2 } };
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
            Mock<Repository> mock = new(() => new Repository(DataFactory.CreateObject<IDataContext>()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<IEntity>())).Callback(() => ++subscriptionRaisedCount);
            sut.AddEntity(new AuthorTestObject() { Id = 11, FirstName = "D" });
            Assert.Single(sut.GetAuthors());
            Assert.Equal(1, subscriptionRaisedCount);
        }

        [Fact]
        public void AddEntityAuthorWithBooksTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(DataFactory.CreateObject<IDataContext>()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<IEntity>())).Callback(() => ++subscriptionRaisedCount);
            BookTestObject book1 = new() { Id = 12, Title = "E" };
            BookTestObject book2 = new() { Id = 13, Title = "F" };
            AuthorTestObject author = new() { Id = 14, LastName = "G", Books = new List<IBook>() { book1, book2 } };
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
            Mock<Repository> mock = new(() => new Repository(DataFactory.CreateObject<IDataContext>()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<IEntity>())).Callback(() => ++subscriptionRaisedCount);
            sut.AddAuthor(new AuthorTestObject() { Id = 11, FirstName = "D" });
            Assert.Single(sut.GetAuthors());
            Assert.Equal(1, subscriptionRaisedCount);
        }

        [Fact]
        public void AddAuthorWithBooksTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(DataFactory.CreateObject<IDataContext>()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<IEntity>())).Callback(() => ++subscriptionRaisedCount);
            BookTestObject book1 = new() { Id = 12, Title = "E" };
            BookTestObject book2 = new() { Id = 13, Title = "F" };
            AuthorTestObject author = new() { Id = 14, LastName = "G", Books = new List<IBook>() { book1, book2 } };
            book1.Authors.Add(author);
            book1.Authors.Add(author);
            sut.AddAuthor(author);
            Assert.Single(sut.GetAuthors());
            Assert.Equal(3, subscriptionRaisedCount);
        }

        [Fact]
        public void AddExistingAuthorTest()
        {
            Repository sut = new(DataFactory.GetExampleContext());
            AuthorTestObject author = new() { Id = 2 };
            int authorCount = sut.GetAuthors().Count;
            IAuthor returned = sut.AddAuthor(author);
            Assert.Same(author, returned);
            Assert.Equal(authorCount, sut.GetAuthors().Count);
        }

        [Fact]
        public void AddExistingBookTest()
        {
            Repository sut = new(DataFactory.GetExampleContext());
            BookTestObject book = new() { Id = 1 };
            int bookCount = sut.GetBooks().Count;
            IBook returned = sut.AddBook(book);
            Assert.Same(book, returned);
            Assert.Equal(bookCount, sut.GetBooks().Count);
        }

        [Fact]
        public void DisposeTest()
        {
            int subscriptionRaisedCount = 0;
            Mock<Repository> mock = new(() => new Repository(DataFactory.CreateObject<IDataContext>()));
            mock.CallBase = true;
            Repository sut = mock.Object;
            mock.Setup(repo => repo.OnNext(It.IsAny<IEntity>())).Callback(() => ++subscriptionRaisedCount);
            sut.AddEntity(new BookTestObject() { Id = 15 });
            Assert.Equal(1, subscriptionRaisedCount);
            sut.Dispose();
            sut.GetType()
                .GetField("_dataContext", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(sut, DataFactory.CreateObject<IDataContext>());
            sut.AddEntity(new BookTestObject() { Id = 16 });
            Assert.Equal(1, subscriptionRaisedCount);
        }

        private static void AddAuthorsInLoop(IRepository repository, Type entityType, int startingId, int count)
        {
            for (int i = startingId; i < startingId + count; ++i)
            {
                IEntity entity = LogicFactory.CreateObject(entityType) as IEntity;
                entity.Id = i;
                repository.AddEntity(entity);
            }
        }

        [Theory]
        [InlineData(typeof(IAuthorDto))]
        [InlineData(typeof(IBookDto))]
        public async Task AddEntityConcurrentTest(Type entityType)
        {
            int entityCount = 2048;
            Repository sut = new(DataFactory.CreateObject<IDataContext>());
            Task[] tasks = new Task[]
            {
                Task.Run(() => AddAuthorsInLoop(sut, entityType, 0, entityCount)),
                Task.Run(() => AddAuthorsInLoop(sut, entityType, entityCount, entityCount)),
            };
            await Task.WhenAll(tasks);
            Assert.Equal(
                tasks.Length * entityCount,
                entityType.Name == nameof(IAuthorDto) ? sut.GetAuthors().Count : sut.GetBooks().Count
            );
        }

        [Fact]
        public async Task AddEntityDeadlockTest()
        {
            int entityCount = 2048;
            Repository sut = new(DataFactory.CreateObject<IDataContext>());
            Task[] tasks = new Task[]
            {
                Task.Run(() => AddAuthorsInLoop(sut, typeof(IAuthorDto), 0, entityCount)),
                Task.Run(() => AddAuthorsInLoop(sut, typeof(IBookDto), entityCount, entityCount)),
            };
            await Task.WhenAll(tasks);
            Assert.Equal(entityCount, sut.GetAuthors().Count);
            Assert.Equal(entityCount, sut.GetBooks().Count);
        }
    }
}
