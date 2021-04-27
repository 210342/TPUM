using System;
using System.Collections.Generic;
using System.Reflection;
using TPUM.Shared.Data;
using TPUM.Shared.Data.Core;
using TPUM.Shared.Data.Entities;
using Xunit;

namespace TPUM.Shared.DataTests
{
    public class DataFactoryTest
    {
        public static IEnumerable<object[]> TypesToTest()
        {
            yield return new[] { typeof(IDataContext), typeof(DataContext) };
            yield return new[] { typeof(IBook), typeof(Book) };
            yield return new[] { typeof(IAuthor), typeof(Author) };
            yield return new[] { typeof(INetworkPacket), typeof(NetworkPacket) };
        }

        [Theory]
        [MemberData(nameof(TypesToTest))]
        public void GetInstanceTest(Type invokingType, Type expectedType)
        {
            MethodInfo createObject = typeof(DataFactory).GetMethod(
                "CreateObject",
                BindingFlags.Public | BindingFlags.Static,
                Type.DefaultBinder,
                new[] { typeof(object[]) },
                Array.Empty<ParameterModifier>()
            );
            var createObjectGeneric = createObject.MakeGenericMethod(invokingType);
            object returned = createObjectGeneric.Invoke(null, new[] { Array.Empty<object>() });
            Assert.NotNull(returned);
            Assert.Equal(returned.GetType(), expectedType);
        }
    }
}
