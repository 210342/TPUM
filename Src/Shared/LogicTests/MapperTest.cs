using System;
using TPUM.Shared.Logic;
using TPUM.Shared.NetworkModel.Core;
using Xunit;

namespace TPUM.Shared.LogicTests
{
    public class MapperTest
    {

        [Fact]
        public void CreateAuthorDtoForAuthor()
        {
            IAuthor author = NetworkModel.Factory.CreateObject<IAuthor>();
            author.Id = 1;
            author.FirstName = "Firstname";
            author.LastName = "Lastname";
            author.NickName = "Nickname";
            Shared.Logic.WebModel.IAuthor authorDto = Mapper.MapEntities<IAuthor, Logic.WebModel.IAuthor>(author);
            Assert.NotNull(authorDto);
            Assert.Equal(author.Id, authorDto.Id);
            Assert.Equal(author.FirstName, authorDto.FirstName);
            Assert.Equal(author.LastName, authorDto.LastName);
            Assert.Equal(author.NickName, authorDto.NickName);
        }

        [Fact]
        public void CreateBookDtoForBook()
        {
            IBook book = NetworkModel.Factory.CreateObject<IBook>();
            book.Id = 1;
            book.Title = "Firstname";
            Shared.Logic.WebModel.IBook bookDto = Mapper.MapEntities<IBook, Logic.WebModel.IBook>(book);
            Assert.NotNull(bookDto);
            Assert.Equal(book.Id, bookDto.Id);
            Assert.Equal(book.Title, bookDto.Title);
        }

        [Fact]
        public void MapNullObject()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => Mapper.MapEntities<object, object>(null));
            Assert.Equal("entity", exception.ParamName);
        }
    }
}
