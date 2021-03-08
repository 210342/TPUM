using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using TPUM.Model;
using TPUM.Model.Entities;

namespace TPUM.Connectivity
{
    public class HttpClient : IDisposable
    {
        private readonly IRepository _repository;
        private readonly System.Net.Http.HttpClient _httpClient;

        public Uri ServerUrl { get; }

        public HttpClient(string baseUrl, IRepository repository)
        {
            ServerUrl = new Uri(baseUrl);
            _httpClient = new System.Net.Http.HttpClient();
            _repository = repository;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var response = await _httpClient.GetAsync($"{ServerUrl}books");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonSerializer.Deserialize<List<Book>>(await response.Content.ReadAsStringAsync());
            }
            return _repository.GetBooks();
        }

        public async Task<List<Author>> GetAuthorsAsync()
        {
            var response = await _httpClient.GetAsync($"{ServerUrl}authors");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonSerializer.Deserialize<List<Author>>(await response.Content.ReadAsStringAsync());
            }
            return _repository.GetAuthors();
        }

        #region IDisposable

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
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
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
