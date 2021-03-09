using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace API.DTO.v1
{
    public class CommentApiDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string Content { get; set; } = default!;

        public Guid FeatureId { get; set; } = default!;
        // public FeatureApiDto? Feature { get; set; }

        public Guid AppUserId { get; set; } = default!;
        // public AppUserApiDto? AppUser { get; set; }
        
        public DateTime TimeCreated { get; set; }
    }
}