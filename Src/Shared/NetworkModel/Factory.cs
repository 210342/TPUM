using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using TPUM.Shared.NetworkModel.Core;

namespace TPUM.Shared.NetworkModel
{
    public static class Factory
    {
        public static ISerializer<T> CreateSerializer<T>(Format format, Encoding encoding) where T : class
        {
            return new Serializer<T>(encoding, format);
        }

        public static T CreateObject<T>(params object[] @params) where T : class
        {
            Type matchingType = typeof(Factory)
                .Assembly
                .GetTypes()
                .Where(t => typeof(T).IsAssignableFrom(t)
                    && !t.IsAbstract
                    && !t.IsInterface)
                .FirstOrDefault();

            return matchingType == null
                ? default
                : Activator.CreateInstance(
                    matchingType,
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                    null,
                    @params,
                    CultureInfo.InvariantCulture
                ) as T;
        }
    }
}
