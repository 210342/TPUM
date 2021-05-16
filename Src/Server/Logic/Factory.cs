using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using TPUM.Server.Data.Entities;
using TPUM.Server.Logic.Core;
using TPUM.Shared.Logic;
using TPUM.Shared.Logic.Core;

namespace TPUM.Server.Logic
{
    public static class Factory
    {
        public static IRepository CreateRepository()
        {
            return CreateRepository(Enumerable.Empty<IAuthor>(), Enumerable.Empty<IBook>());
        }

        public static IRepository CreateRepository(IEnumerable<IAuthor> authors, IEnumerable<IBook> books)
        {
            return new Repository(Data.Factory.CreateObject<Data.Core.IDataContext>(authors, books));
        }

        public static IRepository GetExampleRepository()
        {
            return new Repository(Data.Factory.GetExampleContext());
        }

        public static INetworkNode CreateNetworkNode(
            Uri uri,
            IRepository repository,
            Func<HttpListenerContext, IRepository, IHttpResponseHandler> httpHandlerFactory,
            Func<HttpListenerContext, CancellationToken, IWebSocketResponseHandler> webSocketHandlerFactory,
            Format format,
            Encoding encoding)
        {
            repository = repository ?? GetExampleRepository();
            return new HttpServer(uri, repository, httpHandlerFactory, webSocketHandlerFactory, format, encoding);
        }

        internal static Shared.Logic.WebModel.IEntity MapEntityToWebModel(Data.Core.IEntity entity)
        {
            if (entity is IAuthor author)
            {
                return Mapper.MapEntities<IAuthor, Shared.Logic.WebModel.IAuthor>(author);
            }
            else if (entity is IBook book)
            {
                return Mapper.MapEntities<IBook, Shared.Logic.WebModel.IBook>(book);
            }
            else
            {
                return null;
            }
        }
    }
}
