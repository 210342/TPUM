using System;
using System.Collections.Generic;
using TPUM.Shared.Data.Entities;

namespace TPUM.Shared.Data.Core
{
    public interface IDataContext : IObservable<IEntity>
    {
        IReadOnlyCollection<IBook> Books { get; }
        IReadOnlyCollection<IAuthor> Authors { get; }
    }
}
