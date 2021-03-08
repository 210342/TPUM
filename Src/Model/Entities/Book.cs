using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TPUM.Model.Core;

namespace TPUM.Model.Entities
{
    [Guid("B093A245-0E2D-4E82-A862-4D5567E0F84E")]
    public class Book : Entity
    {
        public List<Author> Authors { get; set; }
        public string Title { get; set; }
    }
}
