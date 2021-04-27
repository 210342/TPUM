using System.Collections.Generic;
using System.Linq;
using TPUM.Shared.Data.Entities;

namespace TPUM.Shared.Logic.Dto
{
    internal class AuthorDto : IAuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public List<IBook> Books { get; set; } = new List<IBook>();
        public IEnumerable<IBookDto> BookDtos => Books.OfType<IBookDto>();
        public int EntityId => Id;


        public AuthorDto() { }

        public AuthorDto(IAuthor author) : this(author, true)
        {
        }

        public AuthorDto(IAuthor author, bool recursive)
        {
            Id = author.Id;
            FirstName = author.FirstName;
            LastName = author.LastName;
            NickName = author.NickName;
            Books = recursive
                ? author.Books.Select(b => new BookDto(b, false)).OfType<IBook>().ToList()
                : author.Books.ToList();
        }
    }
}
