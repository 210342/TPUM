using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUM.Server.Data;
using TPUM.Server.Data.Entities;
using TPUM.Server.Logic;
using TPUM.Shared.Logic;
using TPUM.Shared.Logic.Core;
using Xunit;

namespace TPUM.Server.LogicTests
{
    public class LogicFactoryTest
    {
        private class Author : IAuthor
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string NickName { get; set; }
            public List<IBook> Books { get; set; } = new List<IBook>();
            public int Id { get; set; }
        }

        private class Book : IBook
        {
            public string Title { get; set; }
            public List<IAuthor> Authors { get; set; } = new List<IAuthor>();
            public int Id { get; set; }
        }

        [Fact]
        public void CreateEmptyRepository()
        {
            IRepository repo = LogicFactory.CreateRepository();
            Assert.NotNull(repo);
            Assert.Empty(repo.GetAuthors());
            Assert.Empty(repo.GetBooks());
        }

        [Fact]
        public void CreateExampleRepository()
        {
            IRepository repo = LogicFactory.GetExampleRepository();
            Assert.NotNull(repo);
            Assert.NotEmpty(repo.GetAuthors());
            Assert.NotEmpty(repo.GetBooks());
        }

        [Fact]
        public void CreateRepository_NullParams()
        {
            IRepository repo = LogicFactory.CreateRepository(null, null);
            Assert.NotNull(repo);
            Assert.Empty(repo.GetAuthors());
            Assert.Empty(repo.GetBooks());
        }

        [Fact]
        public void CreateRepository_NonEmptyParams()
        {
            List<IAuthor> authors = new()
            {
                new Author() { Id = 1 },
                new Author() { Id = 2 },
            };
            List<IBook> books = new()
            {
                new Book() { Id = 1 },
                new Book() { Id = 2, Authors = authors.OfType<IAuthor>().ToList() },
            };
            IRepository repo = LogicFactory.CreateRepository(authors, books);
            Assert.NotNull(repo);
            Assert.Equal(authors.Count, repo.GetAuthors().Count());
            Assert.Equal(books.Count, repo.GetBooks().Count());
            Assert.Equal(books[1].Authors.Count, repo.GetBooks().FirstOrDefault(b => b.Authors.Any())?.Authors?.Count);
        }

        [Fact]
        public void CreateAuthorDtoForAuthor()
        {
            IAuthor author = DataFactory.CreateObject<IAuthor>();
            author.Id = 1;
            author.FirstName = "Firstname";
            author.LastName = "Lastname";
            author.NickName = "Nickname";
            Shared.Logic.WebModel.IEntity entityDto = LogicFactory.MapEntityToWebModel(author);
            Assert.NotNull(entityDto);
            Assert.Equal(author.Id, entityDto.Id);
            Shared.Logic.WebModel.IAuthor authorDto = entityDto as Shared.Logic.WebModel.IAuthor;
            Assert.NotNull(authorDto);
            Assert.Equal(author.Id, authorDto.Id);
            Assert.Equal(author.FirstName, authorDto.FirstName);
            Assert.Equal(author.LastName, authorDto.LastName);
            Assert.Equal(author.NickName, authorDto.NickName);
        }

        [Fact]
        public void CreateBookDtoForBook()
        {
            IBook book = DataFactory.CreateObject<IBook>();
            book.Id = 1;
            book.Title = "Firstname";
            Shared.Logic.WebModel.IEntity entityDto = LogicFactory.MapEntityToWebModel(book);
            Assert.NotNull(entityDto);
            Assert.Equal(book.Id, entityDto.Id);
            Shared.Logic.WebModel.IBook bookDto = entityDto as Shared.Logic.WebModel.IBook;
            Assert.NotNull(bookDto);
            Assert.Equal(book.Id, bookDto.Id);
            Assert.Equal(book.Title, bookDto.Title);
        }

        [Fact]
        public void CreateNullDto()
        {
            Assert.Null(LogicFactory.MapEntityToWebModel(null));
        }

        [Fact]
        public void CreateNetworkNodeTest_Server()
        {
            Uri uri = new("http://example.com");
            INetworkNode node = LogicFactory.CreateNetworkNode(
                uri,
                LogicFactory.CreateRepository(),
                (a, b) => null,
                (a, b) => null,
                Format.JSON,
                Encoding.Default
            );
            Assert.NotNull(node);
            HttpServer server = node as HttpServer;
            Assert.NotNull(server);
            Assert.NotNull(server.Serializer);
            Assert.Equal(uri, server.BaseUri);
        }

        [Fact]
        public void CreateNetworkNodeTest_NullUri()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => LogicFactory.CreateNetworkNode(
                null,
                LogicFactory.CreateRepository(),
                (a, b) => null,
                (a, b) => null,
                Format.JSON,
                Encoding.Default
            ));
            Assert.Equal("uri", exception.ParamName);
        }

        [Fact]
        public void CreateNetworkNodeTest_NullRepository()
        {
            Uri uri = new("http://example.com");
            IRepository emptyRepo = LogicFactory.CreateRepository();
            INetworkNode node = LogicFactory.CreateNetworkNode(
                uri,
                null,
                (a, b) => null,
                (a, b) => null,
                Format.JSON,
                Encoding.Default
            );
            Assert.NotNull(node);
        }

        [Fact]
        public void CreateNetworkNodeTest_NullEncoding()
        {
            Uri uri = new("http://example.com");
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => LogicFactory.CreateNetworkNode(
                uri,
                LogicFactory.CreateRepository(),
                (a, b) => null,
                (a, b) => null,
                Format.JSON,
                null
            ));
            Assert.Equal("encoding", exception.ParamName);
        }

        [Fact]
        public void CreateNetworkNodeTest_NullHttpFactory()
        {
            Uri uri = new("http://example.com");
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => LogicFactory.CreateNetworkNode(
                uri,
                LogicFactory.CreateRepository(),
                null,
                (a, b) => null,
                Format.JSON,
                Encoding.Default
            ));
            Assert.Equal("httpHandlerFactory", exception.ParamName);
        }

        [Fact]
        public void CreateNetworkNodeTest_NullWebSocketFactory()
        {
            Uri uri = new("http://example.com");
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => LogicFactory.CreateNetworkNode(
                uri,
                LogicFactory.CreateRepository(),
                (a, b) => null,
                null,
                Format.JSON,
                Encoding.Default
            ));
            Assert.Equal("webSocketHandlerFactory", exception.ParamName);
        }
    }
}
