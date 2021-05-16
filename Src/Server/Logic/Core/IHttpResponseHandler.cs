using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Server.Logic.Core
{
    public interface IHttpResponseHandler
    {
        bool Handle(Func<IEntity, byte[]> serializer, Func<IEnumerable<IEntity>, byte[]> arraySerializer);
    }
}
