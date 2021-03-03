using System;

namespace Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
        
        public string Content { get; set; } = default!;

        public Guid FeatureId { get; set; } = default!;
        public Feature? Feature { get; set; }

        public Guid AppUserId { get; set; } = default!;
        public AppUser? AppUser { get; set; }
        
        public DateTime TimeCreated { get; set; }
    }
}
