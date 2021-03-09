using System;
using System.Collections.Generic;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace BLL.App.DTO
{
    public class FeatureStatusBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; } = default!;
        
        public string? Description { get; set; }

        // public ICollection<FeatureBllDto>? Features { get; set; }
    }
}