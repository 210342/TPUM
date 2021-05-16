using System;
using System.Collections.Generic;
using System.Linq;
using TPUM.Shared.NetworkModel.Core;

namespace TPUM.Shared.NetworkModel.Formatters
{
    public static class FormatterFactory
    {
        public static IFormatter<T> CreateFormatter<T>(Format format)
        {
            return CreateFormatter<T>(
                format, 
                typeof(T).Assembly.GetTypes().Where(t => typeof(T).IsAssignableFrom(t))
            );
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
