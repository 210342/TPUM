﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPUM.Model.Core;
using TPUM.Model.Entities;

namespace TPUM.Model
{
    public interface IRepository : IObservable<Entity>, IDisposable
    {
        Author AddAuthor(Author author);

        Book AddBook(Book book);

        void AddEntity(object entity);

        List<Book> GetBooks();
        Task<List<Book>> GetBooksAsync();

        List<Author> GetAuthors();
        Task<List<Author>> GetAuthorsAsync();

        Author GetAuthorById(int id);

        Book GetBookById(int id);

        void UpdateBooks(List<Book> books);
        void UpdateAuthors(List<Author> authors);
    }
}
