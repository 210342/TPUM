using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TPUM.Server.Data.Core;
using TPUM.Server.Data.Entities;

namespace TPUM.Server.Data
{
    public static class ObjectCreation
    {
        private static readonly Random _rng = new Random();

        public static IAuthor AddAuthor(IDataContext dataContext)
        {
            DataContext context = dataContext as DataContext;
            Author newAuthor = null;
            Task.Run(() =>
            {
                int id;
                do
                {
                    id = _rng.Next(5000);
                }
                while (context.AuthorsCollection.FirstOrDefault(a => a.Id == id) != null);

                newAuthor = new Author
                {
                    Id = id,
                    FirstName = $"{id} - {nameof(IAuthor.FirstName)}",
                    LastName = $"{id} - {nameof(IAuthor.LastName)}",
                    NickName = $"{id} - {nameof(IAuthor.NickName)}"
                };
                context.AuthorsCollection.Add(newAuthor);
            });
            return newAuthor;
        }

        public static void AddBookInLoop(IDataContext dataContext)
        {
            DataContext context = dataContext as DataContext;
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    int id;
                    do
                    {
                        id = _rng.Next(5000);
                    }
                    while (context.BooksCollection.FirstOrDefault(b => b.Id == id) != null);
                    Book newBook = new Book
                    {
                        Id = id,
                        Title = $"{id} - {nameof(Book.Title)}"
                    };
                    context.BooksCollection.Add(newBook);
                }
            });
        }
    }

}
