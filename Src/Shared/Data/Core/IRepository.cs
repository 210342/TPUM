using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPUM.Shared.Data.Entities;

namespace TPUM.Shared.Data.Core
{
    public interface IRepository : IObservable<IEntity>, IDisposable
    {
        IAuthor AddAuthor(IAuthor author);

        IBook AddBook(IBook book);

        object AddEntity(object entity);

        List<IBook> GetBooks();
        Task<List<IBook>> GetBooksAsync();

        List<IAuthor> GetAuthors();
        Task<List<IAuthor>> GetAuthorsAsync();

        IAuthor GetAuthorById(int id);

        IBook GetBookById(int id);

        void UpdateBooks(List<IBook> books);
        void UpdateAuthors(List<IAuthor> authors);
    }
}
