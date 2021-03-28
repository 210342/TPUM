using System;
using System.Collections.Generic;
using TPUM.Shared.Model.Core;
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
                .EnsureRoundtrip()
                .IgnoreFields();
            DeserializerBuilder deserializerBuilder = new DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
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
            return _serializer.Serialize(obj);
        }

        public override T Deformat(string str)
        {
            return _deserializer.Deserialize<T>(str);
        }
    }
}
