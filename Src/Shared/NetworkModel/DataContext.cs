﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using TPUM.Shared.NetworkModel.Core;

namespace TPUM.Shared.NetworkModel
{
    internal class DataContext : Observable<IEntity>, IDataContext
    {
        private readonly ObservableCollection<Author> _authors = new ObservableCollection<Author>();
        private readonly ObservableCollection<Book> _books = new ObservableCollection<Book>();

        internal ICollection<Book> BooksCollection => _books;
        internal ICollection<Author> AuthorsCollection => _authors;

        public IReadOnlyCollection<IBook> Books => _books;
        public IReadOnlyCollection<IAuthor> Authors => _authors;

        internal DataContext()
        {
            _authors.CollectionChanged += CollectionChanged;
            _books.CollectionChanged += CollectionChanged;
        }

        internal DataContext(IEnumerable<IAuthor> authors, IEnumerable<IBook> books) : this()
        {
            foreach (Author author in authors?.OfType<Author>() ?? Enumerable.Empty<Author>())
            {
                _authors.Add(author);
            }
            foreach (Book book in books?.OfType<Book>() ?? Enumerable.Empty<Book>())
            {
                _books.Add(book);
            }
        }

        public void AddAuthor(IAuthor author)
        {
            _authors.Add(author as Author ?? throw new ArgumentNullException(nameof(author)));
        }

        public void AddBook(IBook book)
        {
            _books.Add(book as Book ?? throw new ArgumentNullException(nameof(book)));
        }

        public void ClearAuthors()
        {
            _authors.Clear();
        }
        public void ClearBooks()
        {
            _books.Clear();
        }
        public void UpdateBooks(IEnumerable<IBook> books)
        {
            IEnumerable<Book> booksToAdd = books.Select(b => new Book()
            {
                Id = b.Id,
                Title = b.Title,
                Authors = b.Authors
            });
            foreach (Book book in booksToAdd)
            {
                AddBook(book);
            }
        }
        public void UpdateAuthors(IEnumerable<IAuthor> authors)
        {
            IEnumerable<Author> authorsToAdd = authors.Select(a => new Author()
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                NickName = a.NickName,
                Books = a.Books
            });
            foreach (Author author in authorsToAdd)
            {
                AddAuthor(author);
            }
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
            => InvokeEntitiesChanged(args.NewItems?.OfType<Entity>() ?? Enumerable.Empty<Entity>());

        #region IDisposable

        private bool _disposedValue;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _authors.CollectionChanged -= CollectionChanged;
                    _books.CollectionChanged -= CollectionChanged;
                    _authors.Clear();
                    _books.Clear();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        #endregion


    }
}
