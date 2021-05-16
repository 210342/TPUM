using System;
using System.Text;
using TPUM.Shared.Logic;
using TPUM.Shared.Logic.Core;

namespace TPUM.Client.Logic
{
    public static class Factory
    {
        public static IRepository CreateRepository(Uri serverUri, Format format, Encoding encoding)
        {
            return new WebRepository(serverUri, Mapper.MapFormat(format), encoding);
        }
    }
}
