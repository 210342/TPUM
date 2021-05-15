using System;
using System.Collections.Generic;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Server.Logic.Core
{
    public interface IHttpResponseHandler
    {
        bool Handle(Func<IEnumerable<IEntity>, byte[]> serializer);
    }
}
