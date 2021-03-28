using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPUM.Shared.Model.Core;
using TPUM.Shared.Model.Formatters;

namespace TPUM.Shared.Model
{
    public class Serializer<T> : ISerializer<T>
        where T: class
    {
        public Encoding Encoding { get; }
        public IFormatter<T> Formatter { get; }

        public Serializer(Encoding encoding, Format format)
        {
            Encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            Formatter = FormatterFactory.CreateFormatter<T>(format);
        }

        public Serializer(Encoding encoding, Format format, IEnumerable<Type> knownTypes) : this(encoding, FormatterFactory.CreateFormatter<T>(format, knownTypes)) { }

        public Serializer(Encoding encoding, IFormatter<T> formatter)
        {
            Encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        public byte[] Serialize(T obj)
        {
            _ = obj ?? throw new ArgumentNullException(nameof(obj));
            return Encoding.GetBytes(Formatter.FormatObject(obj));
        }

        public T Deserialize(byte[] bytes)
        {
            if (!bytes?.Any() ?? true)
            {
                throw new ArgumentException("Bytes array is empty or null", nameof(bytes));
            }
            return Formatter.Deformat(Encoding.GetString(bytes));
        }
    }
}
