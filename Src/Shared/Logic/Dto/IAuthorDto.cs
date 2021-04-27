using System.Collections.Generic;
using TPUM.Shared.Data.Entities;

namespace TPUM.Shared.Logic.Dto
{
    public interface IAuthorDto : IAuthor, IEntityDto
    {
        IEnumerable<IBookDto> BookDtos { get; }
    }
}
