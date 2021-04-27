using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using TPUM.Shared.Data;
using TPUM.Shared.Data.Core;
using TPUM.Shared.Data.Entities;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.Dto;

namespace TPUM.Shared.Logic
{
    public static class LogicFactory
    {
        public static IRepository CreateRepository()
        {
            return CreateRepository(Enumerable.Empty<IAuthor>(), Enumerable.Empty<IBook>());
        }

        public static IRepository CreateRepository(IEnumerable<IAuthor> authors, IEnumerable<IBook> books)
        {
            return new Repository(DataFactory.CreateObject<IDataContext>(authors, books));
        }

        public static IRepository GetExampleRepository()
        {
            return new Repository(DataFactory.GetExampleContext());
        }

        public static T CreateObject<T>(params object[] @params) where T: class
        {
            Type matchingType = typeof(LogicFactory)
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
            Type matchingType = typeof(LogicFactory)
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

        public static object CreateDtoObjectForEntity(IEntity entity)
        {
            if (entity is IAuthor author)
            {
                return new AuthorDto(author);
            }
            if (entity is IBook book)
            {
                return new BookDto(book);
            }
            return default;
        }
    }
}
