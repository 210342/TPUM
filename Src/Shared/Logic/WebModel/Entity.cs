using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using YamlDotNet.Serialization;

namespace TPUM.Shared.Logic.WebModel
{
    [Guid("ABE70968-E4A3-4349-9CE3-FDCD529F0081")]
    [DataContract(Name = "Entity", Namespace = "http://tpum.example.com", IsReference = true)]
    internal class Entity : IExtensibleDataObject, IEntity
    {
        [DataMember]
        public int Id { get; set; }

        #region Serialization support

        [YamlIgnore]
        [JsonIgnore]
        public ExtensionDataObject ExtensionData { get; set; }
        [YamlIgnore]
        [JsonExtensionData]
        public Dictionary<string, object> JsonExtensionData { get; set; }

        #endregion

        public override bool Equals(object obj)
        {
            return obj is Entity entity &&
                   Id == entity.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
