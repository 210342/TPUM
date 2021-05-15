using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace TPUM.Shared.Logic
{
    public static class Factory
    {
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

        public static object CreateObject(Type type, params object[] @params)
        {
            Type matchingType = typeof(Factory)
                .Assembly
                .GetTypes()
                .Where(t => type.IsAssignableFrom(t)
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
               );
        }
    }
}
