using System;
using System.Linq;

namespace TPUM.Shared.Data
{
    public static class DataFactory
    {
        public static T CreateObject<T>(params object[] @params) where T : class
        {
            Type matchingType = typeof(DataFactory)
                .Assembly
                .GetTypes()
                .Where(t => typeof(T).IsAssignableFrom(t)
                    && !t.IsAbstract
                    && !t.IsInterface)
                .FirstOrDefault();

            return matchingType == null ? default : Activator.CreateInstance(matchingType, @params) as T;
        }

        public static T CreateObject<T>(string typeName, params object[] @params) where T : class
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException(nameof(typeName));
            }
            Type matchingType = typeof(DataFactory)
                .Assembly
                .GetTypes()
                .Where(t => typeof(T).IsAssignableFrom(t)
                    && !t.IsAbstract
                    && !t.IsInterface
                    && t.Name.Equals(typeName))
                .FirstOrDefault();

            return matchingType == null ? default : Activator.CreateInstance(matchingType, @params) as T;
        }
    }
}
