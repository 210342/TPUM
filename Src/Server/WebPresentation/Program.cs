using System;
using System.Text;
using System.Threading;
using TPUM.Server.Logic;
using TPUM.Shared.Logic.Core;

namespace TPUM.Server.WebPresentation
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 5000;
            if (args.Length > 1) 
            {
                _ = int.TryParse(args[1], out port);
            }
            using INetworkNode server = LogicFactory.CreateNetworkNode(
                new Uri($"http://localhost:{port}"),
                LogicFactory.GetExampleRepository(),
                (context, repository) => new HttpResponseHandler(context, repository),
                (context, token) => new WebSocketResponseHandler(context, token),
                Format.JSON,
                Encoding.UTF8
            );
            server.Start();
            Thread.Sleep(60000);
            server.Stop();
        }
    }
}
