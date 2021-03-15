using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace BLL.App.DTO
{
    public class CommentBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string Content { get; set; } = default!;

        public Guid FeatureId { get; set; } = default!;
        public FeatureBllDto? Feature { get; set; }

        public Guid AppUserId { get; set; } = default!;
        public AppUserBllDto? AppUser { get; set; }
        
        public DateTime TimeCreated { get; set; }
    }
}