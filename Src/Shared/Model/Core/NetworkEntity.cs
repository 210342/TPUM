using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TPUM.Shared.Model.Core
{
    public class NetworkEntity
    {
        public Uri Source { get; set; }
        public Guid TypeIdentifier { get; set; }
        [JsonIgnore]
        public object Entity { get; set; }

        public string Serialize()
        {
            string json = JsonSerializer.Serialize(this);
            string value = JsonSerializer.Serialize(Entity);
            return $"{json.Substring(0, json.Length -  1)},\"{nameof(Entity)}\":{value}}}";
        }

        public static NetworkEntity Deserialize(string sourceJson)
        {
            JsonElement root = JsonDocument.Parse(sourceJson).RootElement;
            NetworkEntity entity = new NetworkEntity()
            {
                Source = new Uri(root.GetProperty(nameof(Source)).GetString()),
                TypeIdentifier = Guid.Parse(root.GetProperty(nameof(TypeIdentifier)).GetString())
            };
            return entity.DeserializeValue(sourceJson);
        }

        private NetworkEntity DeserializeValue(string sourceJson)
        {
            Type type = GetType().Assembly.ExportedTypes.FirstOrDefault(t => t.GUID == TypeIdentifier);
            JsonElement jsonValue = JsonDocument.Parse(sourceJson).RootElement.GetProperty(nameof(Entity));
            Entity = JsonSerializer.Deserialize(jsonValue.GetRawText(), type);
            return this;
        }
    }
}
