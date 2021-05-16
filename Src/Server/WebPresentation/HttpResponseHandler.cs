using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
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

        public bool Handle(Func<IEntity, byte[]> serializer, Func<IEnumerable<IEntity>, byte[]> arraySerializer)
        {
            HttpListenerResponse response = _context.Response;
            response.StatusCode = 200;
            response.ContentType = "text/plain; charset=utf-8";
            if (_context.Request.RawUrl.ToLower().Contains("disconnect"))
            {
                using (StreamWriter writer = new(response.OutputStream))
                {
                    writer.WriteLine("Closing the server");
                    response.Close();
                }
                return false;
            }
            else if (_context.Request.RawUrl.ToLower().Contains("books"))
            {
                byte[] bytesToWrite = arraySerializer.Invoke(_repository.GetBooks());
                response.OutputStream.Write(bytesToWrite, 0, bytesToWrite.Length);
            }
            else if (_context.Request.RawUrl.ToLower().Contains("authors"))
            {
                byte[] bytesToWrite = arraySerializer.Invoke(_repository.GetAuthors());
                response.OutputStream.Write(bytesToWrite, 0, bytesToWrite.Length);
            }
            else if (_context.Request.RawUrl.ToLower().Contains("add"))
            {
                byte[] bytesToWrite = serializer.Invoke(_repository.AddRandomAuthor().GetAwaiter().GetResult());
                response.OutputStream.Write(bytesToWrite, 0, bytesToWrite.Length);
            }
            response.Close();
            return true;
        }
    }
}
