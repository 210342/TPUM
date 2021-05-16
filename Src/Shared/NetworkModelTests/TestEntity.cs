using System.Runtime.Serialization;

namespace TPUM.Shared.NetworkModelTests
{
    [DataContract(Namespace = "tpum.example.com")]
    public class TestEntity
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Foo { get; set; }
    }
}
