using System;
using System.Collections.Generic;
using TPUM.Shared.Model.Core;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TPUM.Shared.Model.Formatters
{
    public class YamlFormatter<T> : TypesFormatter<T>, IFormatter<T>
    {
        private readonly ISerializer _serializer;
        private readonly IDeserializer _deserializer;

        public override Format Format => Format.YAML;

        public YamlFormatter() : this(new[] { typeof(T) }) { }

        public YamlFormatter(IEnumerable<Type> knownTypes) : base(knownTypes)
        {
            SerializerBuilder serializerBuilder = new SerializerBuilder()
               .WithNamingConvention(PascalCaseNamingConvention.Instance)
               .IgnoreFields()
               .EnsureRoundtrip();
            DeserializerBuilder deserializerBuilder = new DeserializerBuilder()
                 .WithNamingConvention(PascalCaseNamingConvention.Instance)
                 .IgnoreFields();
            foreach (Type type in KnownTypes)
            {
                serializerBuilder = serializerBuilder.WithTagMapping($"!{type.FullName}", type);
                deserializerBuilder = deserializerBuilder.WithTagMapping($"!{type.FullName}", type);
            }
            _serializer = serializerBuilder.Build();
            _deserializer = deserializerBuilder.Build();
        }

        public override string FormatObject(T obj)
        {
            try
            {
                return _serializer.Serialize(obj);
            }
            catch (YamlException ex)
            {
                throw new ArgumentException($"Unregistered object type, serializer expected: {typeof(T).FullName}", obj.GetType().FullName, ex);
            }
        }

        public override T Deformat(string str)
        {
            try
            {
                return _deserializer.Deserialize<T>(str);
            }
            catch (YamlException ex)
            {
                throw new ArgumentException($"Unregistered object type, serializer expected: {typeof(T).FullName}", ex);
            }
        }
    }
}
