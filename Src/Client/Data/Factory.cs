using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using TPUM.Client.Data.Core;
using TPUM.Shared.NetworkModel.Core;

namespace TPUM.Client.Data
{
    public static class Factory
    {
        public static T CreateWebDataSource<T>(Uri uri, Format format, Encoding encoding)
            where T : class, IWebDataSource
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
                    new object[] { uri, format, encoding },
                    CultureInfo.InvariantCulture
                ) as T;
        }
    }
}
