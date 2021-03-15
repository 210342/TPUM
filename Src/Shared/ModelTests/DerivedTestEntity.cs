using System.Runtime.Serialization;

namespace TPUM.Shared.ModelTests
{
    [DataContract(Namespace = "tpum.example.com")]
    public class DerivedTestEntity : TestEntity
    {
        [DataMember]
        public int Count { get; set; }
    }
}
