using AutomationSystem.Base.Contract.Enums;
using System;

namespace AutomationSystem.Base.Contract.Models
{
    /// <summary>
    /// Wraps tuple EntityTypeId and EntityId
    /// </summary>
    public class NullableEntityId
    {
        public EntityTypeEnum? TypeId { get; }
        public long? Id { get; }

        public bool IsNull => !TypeId.HasValue && !Id.HasValue;

        public NullableEntityId() { }

        public NullableEntityId(EntityTypeEnum? entityTypeId, long? entityId)
        {
            if ((entityTypeId.HasValue && !entityId.HasValue)
                || (!entityTypeId.HasValue && entityId.HasValue))
            {
                throw new ArgumentException($"Both parameters {nameof(entityTypeId)} and {nameof(entityId)} should be null or have value.");
            }

            TypeId = entityTypeId;
            Id = entityId;
        }

        public override string ToString()
        {
            if (IsNull)
            {
                return "null";
            }

            return $"{TypeId}({Id})";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is NullableEntityId typedObj))
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
