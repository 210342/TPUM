using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TPUM.Shared.Model.Core;

namespace TPUM.Shared.Model.Entities
{
    [Guid("EB2E9E9F-C340-4244-BC2D-4B9531AEF2DA")]
    public class Author : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();

        public override bool Equals(object obj)
        {
            return obj is Author author &&
                   base.Equals(obj) &&
                   Id == author.Id &&
                   FirstName == author.FirstName &&
                   LastName == author.LastName &&
                   NickName == author.NickName &&
                   EqualityComparer<List<Book>>.Default.Equals(Books, author.Books);
        }

        public override int GetHashCode()
        {
            int hashCode = -707290664;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LastName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(NickName);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Book>>.Default.GetHashCode(Books);
            return hashCode;
        }
    }
}
