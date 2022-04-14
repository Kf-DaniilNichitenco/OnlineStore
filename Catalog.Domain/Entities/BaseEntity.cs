using Catalog.Infrastructure.Entities;

namespace Catalog.Domain.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; }
    }
}
