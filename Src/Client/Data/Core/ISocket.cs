using System;
using System.Threading.Tasks;

namespace TPUM.Client.Data.Core
{
    public interface ISocket : IWebDataSource, IDisposable
    {
        Task Start();
        void Stop();
    }
}
