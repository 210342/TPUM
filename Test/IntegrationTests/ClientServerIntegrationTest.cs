using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TPUM.Client.ViewModel;
using TPUM.Server.Logic;
using TPUM.Server.WebPresentation;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.WebModel;
using Xunit;

namespace TPUM.IntegrationTests
{
    public class ClientServerIntegrationTest
    {
        public static IEnumerable<object[]> WebServiceParams()
        {
            yield return new object[] { Format.JSON, Encoding.ASCII };
            yield return new object[] { Format.JSON, Encoding.UTF8 };
            yield return new object[] { Format.JSON, Encoding.Unicode };
            yield return new object[] { Format.JSON, Encoding.UTF32 };
            yield return new object[] { Format.XML, Encoding.ASCII };
            yield return new object[] { Format.XML, Encoding.UTF8 };
            yield return new object[] { Format.XML, Encoding.Unicode };
            yield return new object[] { Format.XML, Encoding.UTF32 };
            yield return new object[] { Format.YAML, Encoding.ASCII };
            yield return new object[] { Format.YAML, Encoding.UTF8 };
            yield return new object[] { Format.YAML, Encoding.Unicode };
            yield return new object[] { Format.YAML, Encoding.UTF32 };
        }

        [Theory]
        [MemberData(nameof(WebServiceParams))]
        public void LogicToLogicTest(Format format, Encoding encoding)
        {
            int port = 5000;
            Uri uri = new($"http://localhost:{port}");
            using INetworkNode server = Factory.CreateNetworkNode(
                uri,
                Factory.CreateRepository(),
                (context, repository) => new HttpResponseHandler(context, repository),
                (context, token) => new WebSocketResponseHandler(context, token),
                format,
                encoding
            );
            server.Start();
            using IRepository webRepository = Client.Logic.Factory.CreateRepository(uri, format, encoding);
            StockViewModel viewModel = new(webRepository, new TestDispatcherImplementation());
            Assert.Empty(viewModel.Authors);
            viewModel.AddAuthorCommand.Execute(null);
            Thread.Sleep(11000);
            Assert.NotEmpty(viewModel.Authors);
            Assert.NotEmpty(viewModel.Books);
            server.Stop();
        }
    }
}
