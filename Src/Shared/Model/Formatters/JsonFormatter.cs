using System.Text.Json;
using TPUM.Shared.Model.Core;

namespace TPUM.Shared.Model.Formatters
{
    public class JsonFormatter<T> : IFormatter<T>
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            WriteIndented = false,
            PropertyNameCaseInsensitive = true
        };

        public Format Format => Format.JSON;

        public T Deformat(string str)
        {
            return JsonSerializer.Deserialize<T>(str, _options);
        }

        public string FormatObject(T obj)
        {
            return JsonSerializer.Serialize(obj, _options);
        }
    }
}
