using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPUM.Shared.NetworkModel.Core;

namespace TPUM.Client.Data.Core
{
    public interface IHttpClient : IWebDataSource, IDisposable
    {
        Task<IEnumerable<IBook>> GetBooksAsync();
        Task<IEnumerable<IAuthor>> GetAuthorsAsync();
        Task<IAuthor> AddRandomAuthorAsync();
    }
}
