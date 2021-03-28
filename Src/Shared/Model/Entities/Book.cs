using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using TPUM.Shared.Model.Core;

namespace TPUM.Shared.Model.Entities
{
    [Guid("B093A245-0E2D-4E82-A862-4D5567E0F84E")]
    [DataContract(IsReference = true)]
    public class Book : Entity
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public List<Author> Authors { get; set; } = new List<Author>();

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
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Author>>.Default.GetHashCode(Authors);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            return hashCode;
        }
    }
}
