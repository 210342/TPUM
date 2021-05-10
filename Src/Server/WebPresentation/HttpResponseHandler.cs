using System.Collections.Generic;
using System.IO;
using System.Net;
using TPUM.Server.Logic.Core;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Server.WebPresentation
{
    public class HttpResponseHandler : IHttpResponseHandler
    {
        private readonly HttpListenerContext _context;
        private readonly IRepository _repository;

        public HttpResponseHandler(HttpListenerContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public bool Handle(ISerializer<IEnumerable<IEntity>> serializer)
        {
            HttpListenerResponse response = _context.Response;
            response.StatusCode = 200;
            response.ContentType = "text/plain; charset=utf-8";
            using (StreamWriter writer = new(response.OutputStream))
            {
                if (_context.Request.RawUrl.ToLower().Contains("disconnect"))
                {
                    writer.WriteLine("Closing the server");
                    return false;
                }
                else if (_context.Request.RawUrl.ToLower().Contains("books"))
                {
                    byte[] bytesToWrite = serializer.Serialize(_repository.GetBooks());
                    response.OutputStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                }
                else if (_context.Request.RawUrl.ToLower().Contains("authors"))
                {
                    byte[] bytesToWrite = serializer.Serialize(_repository.GetAuthors());
                    response.OutputStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                }
                else if (_context.Request.RawUrl.ToLower().Contains("add"))
                {
                    _repository.AddRandomAuthor();
                }
            }
            response.Close();
            return true;
        }
    }
}
