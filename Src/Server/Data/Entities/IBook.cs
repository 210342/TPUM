using System.Collections.Generic;
using TPUM.Server.Data.Core;

namespace TPUM.Server.Data.Entities
{
    public interface IBook : IEntity
    {
        string Title { get; set; }
        List<IAuthor> Authors { get; set; }
    }
}
