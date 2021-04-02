using System;
using System.Collections.Generic;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace DAL.App.DTO
{
    public class CategoryDalDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = default!;
        
        public string? Description { get; set; }

        public ICollection<FeatureDalDto>? Features { get; set; }
    }
}