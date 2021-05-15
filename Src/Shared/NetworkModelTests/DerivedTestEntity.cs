using System.Runtime.Serialization;

namespace TPUM.Shared.NetworkModelTests
{
    [DataContract(Namespace = "tpum.example.com")]
    public class DerivedTestEntity : TestEntity
    {
        [DataMember]
        public int Count { get; set; }
    }
}
