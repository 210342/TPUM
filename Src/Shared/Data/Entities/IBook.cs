using System.Collections.Generic;
using TPUM.Shared.Data.Core;

namespace TPUM.Shared.Data.Entities
{
    public interface IBook : IEntity
    {
        string Title { get; set; }
        List<IAuthor> Authors { get; set; }
    }
}
