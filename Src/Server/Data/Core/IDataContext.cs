using System;
using System.Collections.Generic;
using TPUM.Server.Data.Entities;

namespace TPUM.Server.Data.Core
{
    public interface IDataContext : IObservable<IEntity>
    {
        IReadOnlyCollection<IBook> Books { get; }
        IReadOnlyCollection<IAuthor> Authors { get; }
        void AddAuthor(IAuthor author);
        void AddBook(IBook book);
        void ClearAuthors();
        void ClearBooks();
        void UpdateBooks(IEnumerable<IBook> books);
        void UpdateAuthors(IEnumerable<IAuthor> authors);
    }
}
