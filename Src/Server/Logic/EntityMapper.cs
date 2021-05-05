using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

namespace TPUM.Server.Logic
{
    internal static class EntityMapper
    {
        private class ContractResolver : DefaultContractResolver
        {
            public override JsonContract ResolveContract(Type type)
            {
                JsonContract contract = base.ResolveContract(type);
                Type outputNonAbstractType = type
                    .Assembly
                    .GetTypes()
                    .Where(t => type.IsAssignableFrom(t)
                        && !t.IsAbstract
                        && !t.IsInterface)
                    .FirstOrDefault();
                if (outputNonAbstractType != null)
                {
                    contract.CreatedType = outputNonAbstractType;
                    contract.DefaultCreator = () => outputNonAbstractType.GetConstructor(Array.Empty<Type>()).Invoke(Array.Empty<Type>());
                }
                return contract;
            }
        }

        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            Formatting = Formatting.None,
            ContractResolver = new ContractResolver()
        };

        public static TOut MapEntities<TIn, TOut>(TIn entity) 
            where TIn : class 
            where TOut : class
        {
            string json = JsonConvert.SerializeObject(entity, typeof(TIn), _settings);
            return JsonConvert.DeserializeObject<TOut>(json, _settings);
        }
    }
}
