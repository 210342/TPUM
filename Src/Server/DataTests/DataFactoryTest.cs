using System;
using System.Collections.Generic;
using System.Reflection;
using TPUM.Server.Data;
using TPUM.Server.Data.Core;
using TPUM.Server.Data.Entities;
using Xunit;

namespace TPUM.Server.DataTests
{
    public class DataFactoryTest
    {
        public static IEnumerable<object[]> TypesToTest()
        {
            yield return new[] { typeof(IDataContext), typeof(DataContext) };
            yield return new[] { typeof(IBook), typeof(Book) };
            yield return new[] { typeof(IAuthor), typeof(Author) };
        }

        [Theory]
        [MemberData(nameof(TypesToTest))]
        public void GetInstanceTest(Type invokingType, Type expectedType)
        {
            MethodInfo createObject = typeof(Factory).GetMethod(
                "CreateObject",
                BindingFlags.Public | BindingFlags.Static,
                Type.DefaultBinder,
                new[] { typeof(object[]) },
                Array.Empty<ParameterModifier>()
            );
            MethodInfo createObjectGeneric = createObject.MakeGenericMethod(invokingType);
            object returned = createObjectGeneric.Invoke(null, new[] { Array.Empty<object>() });
            Assert.NotNull(returned);
            Assert.Equal(returned.GetType(), expectedType);
        }
    }
}
