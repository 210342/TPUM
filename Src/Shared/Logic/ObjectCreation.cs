using System;
using System.Threading;
using System.Threading.Tasks;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.Dto;

namespace TPUM.Shared.Logic
{
    public static class ObjectCreation
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

                IAuthorDto newAuthor = new AuthorDto
                {
                    Id = id,
                    FirstName = $"{id} - {nameof(IAuthorDto.FirstName)}",
                    LastName = $"{id} - {nameof(IAuthorDto.LastName)}",
                    NickName = $"{id} - {nameof(IAuthorDto.NickName)}"
                };
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
                    IBookDto newBook = new BookDto
                    {
                        Id = id,
                        Title = $"{id} - {nameof(IBookDto.Title)}"
                    };
                    repository.AddBook(newBook);
                }
            });
        }
    }

}
