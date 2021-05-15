using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TPUM.Shared.NetworkModel.Core;
using YamlDotNet.Serialization;

namespace TPUM.Shared.NetworkModel
{
    [DataContract]
    internal class NetworkPacket : AbstractNetworkPacket, IExtensibleDataObject
    {
        #region Serialization support

        [JsonIgnore]
        [YamlIgnore]
        public ExtensionDataObject ExtensionData { get; set; }
        [YamlIgnore]
        [JsonExtensionData]
        public Dictionary<string, object> JsonExtensionData { get; set; }

        #endregion

        public override bool Equals(object obj)
        {
            return obj is NetworkPacket entity &&
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
