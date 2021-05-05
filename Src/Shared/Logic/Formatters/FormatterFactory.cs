using System;
using System.Collections.Generic;
using TPUM.Shared.Logic.Core;

namespace TPUM.Shared.Logic.Formatters
{
    public static class FormatterFactory
    {
        public static IFormatter<T> CreateFormatter<T>(Format format)
        {
            return CreateFormatter<T>(format, new[] { typeof(T) });
        }

        public static IFormatter<T> CreateFormatter<T>(Format format, IEnumerable<Type> knownTypes)
        {
            switch (format)
            {
                case Format.XML:
                    return new XmlFormatter<T>(knownTypes);
                case Format.YAML:
                    return new YamlFormatter<T>(knownTypes);
                case Format.JSON:
                    return new JsonFormatter<T>();
                default:
                    return new JsonFormatter<T>();
            }
        }
    }
}
