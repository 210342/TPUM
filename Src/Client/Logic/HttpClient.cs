using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Client.Logic
{
    internal class HttpClient : NetworkNode, IDisposable
    {
        private readonly IRepository _repository;
        private readonly System.Net.Http.HttpClient _httpClient;

        public Uri ServerUri { get; }

        public HttpClient(Uri serverUri, IRepository repository, Format format, Encoding encoding) : base(format, encoding)
        {
            ServerUri = serverUri ?? throw new ArgumentNullException(nameof(serverUri));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _httpClient = new System.Net.Http.HttpClient();
        }

        public override async Task<IEnumerable<IBook>> GetBooksAsync()
        {
            System.Net.Http.HttpResponseMessage response = await _httpClient.GetAsync($"{ServerUri}books");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return _entityListSerializer.Deserialize(await response.Content.ReadAsByteArrayAsync()).OfType<IBook>();
            }
            return _repository.GetBooks();
        }

        public override async Task<IEnumerable<IAuthor>> GetAuthorsAsync()
        {
            System.Net.Http.HttpResponseMessage response = await _httpClient.GetAsync($"{ServerUri}authors");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return _entityListSerializer.Deserialize(await response.Content.ReadAsByteArrayAsync()).OfType<IAuthor>();
            }
            return _repository.GetAuthors();
        }

        public override Task Start() { return Task.CompletedTask; }

        public override void Stop() { }

        public override void Stop(int delay) { }

        #region IDisposable

        private bool disposedValue;
        protected override void Dispose(bool disposing)
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

        #endregion
    }
}
