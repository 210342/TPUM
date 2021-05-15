using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using TPUM.Server.Data.Core;
using TPUM.Server.Data.Entities;

namespace TPUM.Server.Data
{
    public static class Factory
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
