using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace API.DTO.v1
{
    public class CommentCreateApiDto
    {
        public string Content { get; set; } = default!;
        public Guid FeatureId { get; set; } = default!;
    }
    
    public class CommentApiDto : CommentCreateApiDto
    {
        public Guid Id { get; set; }
        public Guid AppUserId { get; set; } = default!;
        public DateTime TimeCreated { get; set; }
        public string User { get; set; } = default!;
    }
}