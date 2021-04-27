using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using TPUM.Shared.Data.Core;
using TPUM.Shared.Data.Entities;

namespace TPUM.Shared.Data
{
    public static class DataFactory
    {
        public static IDataContext GetExampleContext()
        {
            Book book1 = new Book()
            {
                Id = 1,
                Title = "Title1"
            };
            Author author1 = new Author()
            {
                Id = 2,
                FirstName = "FirstName1",
                LastName = "LastName1",
                Books = new List<IBook>() { book1 }
            };
            Author author2 = new Author()
            {
                Id = 3,
                FirstName = "FirstName2",
                LastName = "LastName2",
                Books = new List<IBook>() { book1 }
            };
            Book book2 = new Book()
            {
                Id = 4,
                Title = "title2",
                Authors = new List<IAuthor>() { author1 }
            };
            Book book3 = new Book()
            {
                Id = 5,
                Title = "title3",
                Authors = new List<IAuthor>() { author2 }
            };
            book1.Authors = new List<IAuthor>() { author1, author2 };
            author1.Books.Add(book2);
            author2.Books.Add(book3);
            return new DataContext(
                new List<Author>() { author1, author2 },
                new List<Book>() { book1, book2, book3 }
            );
        }
        public static T CreateObject<T>(params object[] @params) where T : class
        {
            Type matchingType = typeof(DataFactory)
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
        public static object CreateObject(Type type, params object[] @params)
        {
            Type matchingType = typeof(DataFactory)
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
