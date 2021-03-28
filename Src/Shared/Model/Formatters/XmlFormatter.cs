using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using TPUM.Shared.Model.Core;

namespace TPUM.Shared.Model.Formatters
{
    public class XmlFormatter<T> : TypesFormatter<T>, IFormatter<T>
    {
        private readonly DataContractSerializer _serializer;

        public override Format Format => Format.XML;

        public XmlFormatter() : this(new[] { typeof(T) }) { }

        public XmlFormatter(IEnumerable<Type> knownTypes) : base(knownTypes)
        {
            _serializer = new DataContractSerializer(typeof(T), KnownTypes);
        }

        public override string FormatObject(T obj)
        {
            using (Stream stream = new MemoryStream())
            {
                _serializer.WriteObject(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public override T Deformat(string str)
        {
            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(str)))
            {
                object result = _serializer.ReadObject(stream);
                return (T)result;
            }
        }
    }
}
