using Newtonsoft.Json;
using TPUM.Shared.NetworkModel.Core;

namespace TPUM.Shared.NetworkModel.Formatters
{
    internal class JsonFormatter<T> : IFormatter<T>
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            Formatting = Formatting.None,
            TypeNameHandling = TypeNameHandling.All
        };

        public Format Format => Format.JSON;

        public T Deformat(string str)
        {
            return JsonConvert.DeserializeObject<T>(str, _settings);
        }

        public string FormatObject(T obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }
    }
}
