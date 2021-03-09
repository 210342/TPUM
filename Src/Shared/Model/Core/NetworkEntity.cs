using System;
using System.Collections.Generic;
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

        public override bool Equals(object obj)
        {
            return obj is NetworkEntity entity &&
                   EqualityComparer<Uri>.Default.Equals(Source, entity.Source) &&
                   TypeIdentifier.Equals(entity.TypeIdentifier) &&
                   EqualityComparer<object>.Default.Equals(Entity, entity.Entity);
        }

        public override int GetHashCode()
        {
            int hashCode = 2103276648;
            hashCode = hashCode * -1521134295 + EqualityComparer<Uri>.Default.GetHashCode(Source);
            hashCode = hashCode * -1521134295 + TypeIdentifier.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Entity);
            return hashCode;
        }
    }
}
