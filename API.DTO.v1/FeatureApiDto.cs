using System;
using System.Collections.Generic;

namespace API.DTO.v1
{
    public class FeatureCreateApiDto
    {
        public string Title { get; set; } = default!;

        public string? Description { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }

        public int Duration { get; set; }
        
        public string FeatureStatus { get; set; } = default!;

        public Guid? CategoryId { get; set; }
        
        public Guid? AppUserId { get; set; }
    }
    
    public class FeatureEditApiDto : FeatureCreateApiDto
    {
        public Guid Id { get; set; }
        
        public int Size { get; set; }

        public decimal PriorityValue { get; set; }
        
        public DateTime TimeCreated { get; set; }
        
        public string? ChangeLog { get; set; }
    }
    
    public class FeatureApiDto : FeatureEditApiDto
    {
        public DateTime LastEdited { get; set; }
        public string CategoryName { get; set; } = default!;
        public string? Assignee { get; set; }
        public ICollection<Guid>? CommentIds { get; set; }
        public ICollection<Guid>? VotingIds { get; set; }
    }
}