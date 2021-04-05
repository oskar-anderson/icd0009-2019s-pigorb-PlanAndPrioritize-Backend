using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.v1
{
    public class CommentCreateApiDto
    {
        [MaxLength(2048)] [MinLength(1)] public string Content { get; set; } = default!;
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