using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Base.Contract.Models
{
    /// <summary>
    /// Wraps tuple EntityTypeId and EntityId
    /// </summary>
    public class EntityId
    {
        public EntityTypeEnum TypeId { get; }
        public long Id { get; }
        
        public EntityId(EntityTypeEnum entityTypeId, long entityId)
        {
            TypeId = entityTypeId;
            Id = entityId;
        }

        public override string ToString()
        {
            return $"{TypeId}({Id})";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EntityId typedObj))
            {
                return false;
            }

            return TypeId == typedObj.TypeId && Id == typedObj.Id;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
