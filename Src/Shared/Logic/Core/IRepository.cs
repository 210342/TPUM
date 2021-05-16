using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Shared.Logic.Core
{
    public interface IRepository : IObservable<IEntity>, IDisposable
    {
        IAuthor AddAuthor(IAuthor author);
        IBook AddBook(IBook book);
        object AddEntity(object entity);
        Task<IAuthor> AddRandomAuthor();

        IEnumerable<IBook> GetBooks();
        Task<IEnumerable<IBook>> GetBooksAsync();

        IEnumerable<IAuthor> GetAuthors();
        Task<IEnumerable<IAuthor>> GetAuthorsAsync();

        IAuthor GetAuthorById(int id);
        IBook GetBookById(int id);

        void UpdateBooks(List<IBook> books);
        void UpdateAuthors(List<IAuthor> authors);

        bool StartBackgroundWorker();
    }
}
