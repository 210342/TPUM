using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using TPUM.Shared.Logic.Core;
using TPUM.Shared.Logic.WebModel;

namespace TPUM.Shared.Logic
{
    public static class Mapper
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
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            if (!(entity is TIn))
            {
                throw new ArgumentException($"Entity is not of type {typeof(TIn).FullName}", nameof(entity));
            }
            try
            {
                string json = JsonConvert.SerializeObject(entity, typeof(TIn), _settings);
                return JsonConvert.DeserializeObject<TOut>(json, _settings);
            }
            catch (Exception ex)
            {
                throw new IncompatibleMappingException($"Couldn't perform mapping from type {typeof(TIn).FullName} to {typeof(TOut).FullName}", ex);
            }
        }

        public static TOut MapAbstractEntities<TOut>(IEntity entity)
            where TOut : class
        {
            Type typeIn = entity is IAuthor ? typeof(IAuthor) : typeof(IBook);
            try
            {
                string json = JsonConvert.SerializeObject(entity, typeIn, _settings);
                return JsonConvert.DeserializeObject<TOut>(json, _settings);
            }
            catch (Exception ex)
            {
                throw new IncompatibleMappingException($"Couldn't perform mapping from type {typeIn.FullName} to {typeof(TOut).FullName}", ex);
            }
        }

        internal static NetworkModel.Core.Format MapFormat(Format format)
        {
            return (NetworkModel.Core.Format)(int)format;
        }

        public static Format MapFormat(Enum format)
        {
            return Enum.TryParse<Format>(format.ToString(), out Format outputFormat) 
                ? outputFormat 
                : throw new IncompatibleMappingException($"Couldn't perform mapping from {format} to any value of {typeof(Format).FullName}");
        }
    }
}
