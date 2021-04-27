using System;
using System.Threading;
using System.Threading.Tasks;
using TPUM.Shared.Data.Entities;
using TPUM.Shared.Logic.Core;

namespace TPUM.Shared.Logic
{
    public static class Logic
    {
        public static void AddAuthor(IRepository repository)
        {
            Random _rng = new Random();
            Task.Run(() =>
            {
                int id;
                do
                {
                    id = _rng.Next(5000);
                }
                while (repository.GetAuthorById(id) != null);

                IAuthor newAuthor = Data.DataFactory.CreateObject<IAuthor>();
                newAuthor.Id = id;
                newAuthor.FirstName = $"{id} - {nameof(IAuthor.FirstName)}";
                newAuthor.LastName = $"{id} - {nameof(IAuthor.LastName)}";
                newAuthor.NickName = $"{id} - {nameof(IAuthor.NickName)}";
                repository.AddAuthor(newAuthor);
            });
        }

        public static void AddBook(IRepository repository)
        {
            Random _rng = new Random();
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
                    while (repository.GetBookById(id) != null);
                    IBook newBook = Data.DataFactory.CreateObject<IBook>();
                    newBook.Id = id;
                    newBook.Title = $"{id} - {nameof(IBook.Title)}";
                    repository.AddBook(newBook);
                }
            });
        }
    }

}
