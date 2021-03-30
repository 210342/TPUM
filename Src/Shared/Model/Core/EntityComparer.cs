using System;
using System.Collections.Generic;

namespace TPUM.Shared.Model.Core
{
    public class EntityComparer : IEqualityComparer<Entity>, IComparer<Entity>
    {
        public int Compare(Entity x, Entity y)
        {
            if (x is null)
            {
                return -1;
            }
            else if (!(x is null) && y is null)
            {
                return 1;
            }
            Type xType = x.GetType(), yType = y.GetType();
            return xType.Equals(yType)
                ? x.Id - y.Id
                : xType.GUID.CompareTo(yType.GUID);
        }

        public bool Equals(Entity x, Entity y)
        {
            if (x is null || y is null)
            {
                return false;
            }
            return x.GetType().Equals(y.GetType())
                && x.Id == y.Id;
        }

        public int GetHashCode(Entity obj)
        {
            return 84681463 + 36794213 * obj.Id.GetHashCode();
        }
    }
}
