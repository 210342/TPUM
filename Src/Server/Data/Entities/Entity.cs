using TPUM.Server.Data.Core;

namespace TPUM.Server.Data.Entities
{
    internal class Entity : IEntity
    {
        public int Id { get; set; }

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
