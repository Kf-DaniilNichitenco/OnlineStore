using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entities
{
    public abstract class EntityWithOwner : BaseEntity
    {
        public Guid OwnerId { get; set; }
    }
}
