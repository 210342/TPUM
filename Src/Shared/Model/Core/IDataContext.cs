using System;
using System.Collections.Generic;
using System.Text;
using TPUM.Shared.Model.Entities;

namespace TPUM.Shared.Model.Core
{
    public interface IDataContext : IObservable<Entity>
    {
        IReadOnlyCollection<Book> Books { get; }
        IReadOnlyCollection<Author> Authors { get; }
    }
}
