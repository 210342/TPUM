using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using TPUM.Shared.Model.Core;

namespace TPUM.Shared.Model.Entities
{
    [Guid("EB2E9E9F-C340-4244-BC2D-4B9531AEF2DA")]
    [DataContract(IsReference = true)]
    public class Author : Entity
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string NickName { get; set; }
        [DataMember]
        public List<Book> Books { get; set; } = new List<Book>();

        public override bool Equals(object obj)
        {
            return obj is Author author &&
                   base.Equals(obj) &&
                   Id == author.Id &&
                   FirstName == author.FirstName &&
                   LastName == author.LastName &&
                   NickName == author.NickName &&
                   Enumerable.SequenceEqual(Books, author.Books, new EntityComparer());
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
