using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPUM.Server.Data.Entities;
using TPUM.Server.Logic;
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
            IRepository repo = Factory.CreateRepository();
            Assert.NotNull(repo);
            Assert.Empty(repo.GetAuthors());
            Assert.Empty(repo.GetBooks());
        }

        [Fact]
        public void CreateExampleRepository()
        {
            IRepository repo = Factory.GetExampleRepository();
            Assert.NotNull(repo);
            Assert.NotEmpty(repo.GetAuthors());
            Assert.NotEmpty(repo.GetBooks());
        }

        [Fact]
        public void CreateRepository_NullParams()
        {
            IRepository repo = Factory.CreateRepository(null, null);
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
            IRepository repo = Factory.CreateRepository(authors, books);
            Assert.NotNull(repo);
            Assert.Equal(authors.Count, repo.GetAuthors().Count());
            Assert.Equal(books.Count, repo.GetBooks().Count());
            Assert.Equal(books[1].Authors.Count, repo.GetBooks().FirstOrDefault(b => b.Authors.Any())?.Authors?.Count);
        }

        [Fact]
        public void CreateNetworkNodeTest_Server()
        {
            Uri uri = new("http://example.com");
            INetworkNode node = Factory.CreateNetworkNode(
                uri,
                Factory.CreateRepository(),
                (a, b) => null,
                (a, b) => null,
                Format.JSON,
                Encoding.Default
            );
            Assert.NotNull(node);
            HttpServer server = node as HttpServer;
            Assert.NotNull(server);
            Assert.Equal(uri, server.BaseUri);
        }

        [Fact]
        public void CreateNetworkNodeTest_NullUri()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => Factory.CreateNetworkNode(
                null,
                Factory.CreateRepository(),
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
            IRepository emptyRepo = Factory.CreateRepository();
            INetworkNode node = Factory.CreateNetworkNode(
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
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => Factory.CreateNetworkNode(
                uri,
                Factory.CreateRepository(),
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
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => Factory.CreateNetworkNode(
                uri,
                Factory.CreateRepository(),
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
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => Factory.CreateNetworkNode(
                uri,
                Factory.CreateRepository(),
                (a, b) => null,
                null,
                Format.JSON,
                Encoding.Default
            ));
            Assert.Equal("webSocketHandlerFactory", exception.ParamName);
        }
    }
}
