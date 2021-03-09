using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace DAL.App.DTO
{
    public class FeatureStatusDalDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; } = default!;
        
        public string? Description { get; set; }

        // public ICollection<FeatureDalDto>? Features { get; set; }
    }
}