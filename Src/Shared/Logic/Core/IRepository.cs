using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPUM.Shared.Data.Core;
using TPUM.Shared.Data.Entities;
using TPUM.Shared.Logic.Dto;

namespace TPUM.Shared.Logic.Core
{
    public interface IRepository : IObservable<IEntityDto>, IDisposable
    {
        IAuthorDto AddAuthor(IAuthorDto author);

        IBookDto AddBook(IBookDto book);

        object AddEntity(object entity);

        List<IBookDto> GetBooks();
        Task<List<IBookDto>> GetBooksAsync();

        List<IAuthorDto> GetAuthors();
        Task<List<IAuthorDto>> GetAuthorsAsync();

        IAuthorDto GetAuthorById(int id);

        IBookDto GetBookById(int id);

        void UpdateBooks(List<IBookDto> books);
        void UpdateAuthors(List<IAuthorDto> authors);
    }
}
