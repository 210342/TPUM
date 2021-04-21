using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPUM.Shared.Data.Core
{
    public static class DataFactory
    {
        public static T CreateObject<T>(params object[] @params) where T : class
        {
            Type matchingType = typeof(DataFactory)
                .Assembly
                .GetTypes()
                .Where(t => t.IsAssignableFrom(typeof(T))
                    && !t.IsAbstract
                    && !t.IsInterface)
                .FirstOrDefault();

            return matchingType == null ? default : Activator.CreateInstance(matchingType, @params) as T;
        }

        public static T CreateObject<T>(string typeName, params object[] @params) where T: class
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException(nameof(typeName));
            }
            Type matchingType = typeof(DataFactory)
                .Assembly
                .GetTypes()
                .Where(t => t.IsAssignableFrom(typeof(T))
                    && !t.IsAbstract
                    && !t.IsInterface)
                .FirstOrDefault(t => t.Name.Equals(typeName));
            
            return matchingType == null ? default : Activator.CreateInstance(matchingType, @params) as T;
        }
    }
}
