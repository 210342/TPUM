using System;
using System.Collections.Generic;
using TPUM.Shared.Data.Core;

namespace TPUM.Shared.Data
{
    internal class EntityComparer : IEqualityComparer<IEntity>, IComparer<IEntity>
    {
        public int Compare(IEntity x, IEntity y)
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

        public bool Equals(IEntity x, IEntity y)
        {
            if (x is null || y is null)
            {
                return false;
            }
            return x.GetType().Equals(y.GetType())
                && x.Id == y.Id;
        }

        public int GetHashCode(IEntity obj)
        {
            return 84681463 + 36794213 * obj.Id.GetHashCode();
        }
    }
}
