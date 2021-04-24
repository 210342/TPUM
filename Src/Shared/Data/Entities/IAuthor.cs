using System.Collections.Generic;
using TPUM.Shared.Data.Core;

namespace TPUM.Shared.Data.Entities
{
    public interface IAuthor : IEntity
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string NickName { get; set; }
        List<IBook> Books { get; set; }
    }
}
