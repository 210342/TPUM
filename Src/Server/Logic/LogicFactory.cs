using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPUM.Server.Data;
using TPUM.Server.Data.Entities;
using TPUM.Shared.Logic;
using TPUM.Shared.Logic.Core;

namespace TPUM.Server.Logic
{
    public static class LogicFactory
    {
        public static IRepository CreateRepository()
        {
            return CreateRepository(Enumerable.Empty<IAuthor>(), Enumerable.Empty<IBook>());
        }

        public static IRepository CreateRepository(IEnumerable<IAuthor> authors, IEnumerable<IBook> books)
        {
            return new Repository(DataFactory.CreateObject<Data.Core.IDataContext>(authors, books));
        }

        public static IRepository GetExampleRepository()
        {
            return new Repository(DataFactory.GetExampleContext());
        }

        public static INetworkNode CreateNetworkNode(Uri uri, IRepository repository, Format format, Encoding encoding)
        {
            repository = repository ?? GetExampleRepository();
            return new HttpServer(uri, repository, format, encoding);
        }

        internal static Shared.Logic.WebModel.IEntity MapEntityToWebModel(Data.Core.IEntity entity)
        {
            if (entity is IAuthor author)
            {
                return EntityMapper.MapEntities<IAuthor, Shared.Logic.WebModel.IAuthor>(author);
            }
            else if (entity is IBook book)
            {
                return EntityMapper.MapEntities<IBook, Shared.Logic.WebModel.IBook>(book);
            }
            else
            {
                return null;
            }
        }

        internal static Data.Core.IEntity MapWebModelToEntity(Shared.Logic.WebModel.IEntity entity)
        {
            if (entity is Shared.Logic.WebModel.IAuthor author)
            {
                return EntityMapper.MapEntities<Shared.Logic.WebModel.IAuthor, IAuthor>(author);
            }
            else if (entity is Shared.Logic.WebModel.IBook book)
            {
                return EntityMapper.MapEntities<Shared.Logic.WebModel.IBook, IBook>(book);
            }
            else
            {
                return null;
            }
        }
    }
}
