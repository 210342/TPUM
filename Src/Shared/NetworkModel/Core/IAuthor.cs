using System.Collections.Generic;

namespace TPUM.Shared.NetworkModel.Core
{
    public interface IAuthor : IEntity
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string NickName { get; set; }
        List<IBook> Books { get; }
    }
}
