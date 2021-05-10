using System.Collections.Generic;
using TPUM.Server.Data.Core;

namespace TPUM.Server.Data.Entities
{
    public interface IAuthor : IEntity
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string NickName { get; set; }
        List<IBook> Books { get; set; }
    }
}
