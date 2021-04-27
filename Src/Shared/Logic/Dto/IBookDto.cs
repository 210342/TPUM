using System.Collections.Generic;
using TPUM.Shared.Data.Entities;

namespace TPUM.Shared.Logic.Dto
{
    public interface IBookDto : IBook, IEntityDto
    {
        IEnumerable<IAuthorDto> AuthorDtos { get; } 
    }
}
