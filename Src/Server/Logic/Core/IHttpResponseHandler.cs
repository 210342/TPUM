using System;
using System.Collections.Generic;
using System.Text;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Server.Logic.Core
{
    public interface IHttpResponseHandler
    {
        bool Handle(ISerializer<IEnumerable<IEntity>> serializer);
    }
}
