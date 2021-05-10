using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Shared.Logic
{
    public static class SharedLogicFactory
    {
        public static IRepository CreateRepository()
        {
            return CreateRepository(Enumerable.Empty<IAuthor>(), Enumerable.Empty<IBook>());
        }

        public static IRepository CreateRepository(IEnumerable<IAuthor> authors, IEnumerable<IBook> books)
        {
            return new WebRepository(new DataContext(authors, books));
        }

        public static ISerializer<T> CreateSerializer<T>(Format format, Encoding encoding) where T : class
        {
            return new Serializer<T>(encoding, format);
        }

        public static T CreateObject<T>(params object[] @params) where T : class
        {
            Type matchingType = typeof(SharedLogicFactory)
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
            Type matchingType = typeof(SharedLogicFactory)
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
