using System.Collections.Generic;
using System.Linq;

namespace TPUM.Server.Data.Entities
{
    internal class Book : Entity, IBook
    {
        public string Title { get; set; }
        public List<IAuthor> Authors { get; set; } = new List<IAuthor>();

        public Book() { }

        public Book(IBook book)
        {
            Id = book.Id;
            Title = book.Title;
            Authors = book.Authors.ToList();
        }

        public override bool Equals(object obj)
        {
            return obj is Book book &&
                   base.Equals(obj) &&
                   Id == book.Id &&
                   Title == book.Title &&
                   Enumerable.SequenceEqual(Authors, book.Authors, new EntityComparer());
        }

        public override int GetHashCode()
        {
            int hashCode = -27632554;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<IAuthor>>.Default.GetHashCode(Authors);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            return hashCode;
        }
    }
}
