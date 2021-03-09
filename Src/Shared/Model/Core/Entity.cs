using System;
using System.Runtime.InteropServices;

namespace TPUM.Shared.Model.Core
{
    [Guid("ABE70968-E4A3-4349-9CE3-FDCD529F0081")]
    public class Entity
    {
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Entity entity &&
                   Id == entity.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
