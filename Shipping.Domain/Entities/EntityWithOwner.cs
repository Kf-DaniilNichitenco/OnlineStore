namespace Order.Domain.Entities
{
    public abstract class EntityWithOwner : BaseEntity
    {
        public Guid OwnerId { get; set; }
    }
}
