using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace DAL.App.DTO
{
    public class CommentDalDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string Content { get; set; } = default!;

        public Guid FeatureId { get; set; } = default!;
        // public FeatureDalDto? Feature { get; set; }

        public Guid AppUserId { get; set; } = default!;
        public AppUserDalDto? AppUser { get; set; }
        
        public DateTime TimeCreated { get; set; }
    }
}