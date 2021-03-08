using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TPUM.Model.Core;

namespace TPUM.Model.Entities
{
    [Guid("EB2E9E9F-C340-4244-BC2D-4B9531AEF2DA")]
    public class Author : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public List<Book> Books { get; set; }
    }
}
