using System;
using System.Collections.Generic;
using System.Linq;
using TPUM.Shared.Data.Core;

namespace TPUM.Shared.Data.Formatters
{
    internal abstract class TypesFormatter<T> : IFormatter<T>
    {
        public IEnumerable<Type> KnownTypes { get; }
        public abstract Format Format { get; }

        public TypesFormatter(IEnumerable<Type> knownTypes)
        {
            knownTypes = knownTypes?.Where(t => t != null);
            if (knownTypes == null || !knownTypes.Any())
            {
                knownTypes = new[] { typeof(T)};
            }
            KnownTypes = knownTypes.Distinct();
        }

        public abstract string FormatObject(T obj);
        public abstract T Deformat(string str);

        public override bool Equals(object obj)
        {
            return obj is TypesFormatter<T> formatter &&
                   EqualityComparer<IEnumerable<Type>>.Default.Equals(KnownTypes, formatter.KnownTypes);
        }

        public override int GetHashCode()
        {
            return 957872093 + EqualityComparer<IEnumerable<Type>>.Default.GetHashCode(KnownTypes);
        }
    }
}
