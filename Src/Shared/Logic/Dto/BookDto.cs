using System.Collections.Generic;
using System.Linq;
using TPUM.Shared.Data.Entities;

namespace TPUM.Shared.Logic.Dto
{
    class BookDto : IBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<IAuthor> Authors { get; set; } = new List<IAuthor>();
        public IEnumerable<IAuthorDto> AuthorDtos => Authors.OfType<IAuthorDto>();
        public int EntityId => Id;

        public BookDto() { }

        public BookDto(IBook book) : this(book, true) { }

        public BookDto(IBook book, bool recursive)
        {
            Id = book.Id;
            Title = book.Title;
            Authors = recursive
                ? book.Authors.Select(a => new AuthorDto(a, false)).OfType<IAuthor>().ToList()
                : book.Authors.ToList();
        }
    }
}
