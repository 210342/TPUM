using System.Collections.Generic;

namespace TPUM.Shared.Logic.WebModel
{
    public interface IBook : IEntity
    {
        string Title { get; set; }
        List<IAuthor> Authors { get; }
    }
}
