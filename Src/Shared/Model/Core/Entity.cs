using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace TPUM.Shared.Model.Core
{
    [Guid("ABE70968-E4A3-4349-9CE3-FDCD529F0081")]
    [DataContract(Name = "Entity", Namespace = "http://tpum.example.com")]
    public class Entity : IExtensibleDataObject
    {
        [DataMember]
        public int Id { get; set; }
        public ExtensionDataObject ExtensionData { get; set; }

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
