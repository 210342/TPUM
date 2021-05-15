using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Shared.Logic.Core
{
    public interface INetworkNode : IDisposable
    {
        Task Start();
        void Stop();
        void Stop(int delay);

        Task<IEnumerable<IBook>> GetBooksAsync();
        Task<IEnumerable<IAuthor>> GetAuthorsAsync();
    }
}
