using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUM.Client.Data.Core;
using TPUM.Shared.NetworkModel.Core;

namespace TPUM.Client.Data
{
    internal class HttpClient : IHttpClient
    {
        private readonly ISerializer<IEntity[]> _entityListSerializer;
        private readonly ISerializer<IEntity> _entitySerializer;
        private readonly System.Net.Http.HttpClient _httpClient;

        public Uri ServerUri { get; }

        public HttpClient(Uri serverUri, Format format, Encoding encoding)
        {
            ServerUri = serverUri ?? throw new ArgumentNullException(nameof(serverUri));
            _httpClient = new System.Net.Http.HttpClient();
            _entityListSerializer = Shared.NetworkModel.Factory.CreateSerializer<IEntity[]>(format, encoding);
            _entitySerializer = Shared.NetworkModel.Factory.CreateSerializer<IEntity>(format, encoding);
        }

        public async Task<IEnumerable<IBook>> GetBooksAsync()
        {
            System.Net.Http.HttpResponseMessage response = await _httpClient.GetAsync($"{ServerUri}books");
            return _entityListSerializer.Deserialize(await response.Content.ReadAsByteArrayAsync()).OfType<IBook>();
        }

        public async Task<IEnumerable<IAuthor>> GetAuthorsAsync()
        {
            System.Net.Http.HttpResponseMessage response = await _httpClient.GetAsync($"{ServerUri}authors");
            return _entityListSerializer.Deserialize(await response.Content.ReadAsByteArrayAsync()).OfType<IAuthor>();
        }

        public async Task<IAuthor> AddRandomAuthorAsync()
        {
            System.Net.Http.HttpResponseMessage response = await _httpClient.GetAsync($"{ServerUri}add");
            return _entitySerializer.Deserialize(await response.Content.ReadAsByteArrayAsync()) as IAuthor;
        }

        #region IDisposable

        private bool disposedValue;
        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _httpClient.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~HttpClient()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
