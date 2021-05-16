using System.Collections.Generic;

namespace TPUM.Shared.NetworkModel.Core
{
    public interface IBook : IEntity
    {
        string Title { get; set; }
        List<IAuthor> Authors { get; }
    }
}
