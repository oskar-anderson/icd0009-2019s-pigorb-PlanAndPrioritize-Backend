using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.v1
{
    public class FeatureCreateApiDto
    {
        [MaxLength(512)] [MinLength(1)] public string Title { get; set; } = default!;

        [MaxLength(2048)] public string? Description { get; set; }
        
        [DataType(DataType.Date)] public DateTime? StartTime { get; set; }
        
        [DataType(DataType.Date)] public DateTime? EndTime { get; set; }
        
        public Guid CategoryId { get; set; }
        
        public Guid? AppUserId { get; set; }
    }
    
    public class FeatureEditApiDto : FeatureCreateApiDto
    {
        public Guid Id { get; set; }
        public string FeatureStatus { get; set; } = default!;
    }
    
    public class FeatureApiDto : FeatureEditApiDto
    {
        public int Duration { get; set; }
        public DateTime LastEdited { get; set; }
        public string CategoryName { get; set; } = default!;
        public string? Assignee { get; set; }
        
        public int Size { get; set; }

        public decimal PriorityValue { get; set; }
        
        public DateTime TimeCreated { get; set; }

        public string CreatedBy { get; set; } = default!;
        public string? ChangeLog { get; set; }
        public ICollection<Guid>? CommentIds { get; set; }
        public ICollection<Guid>? VotingIds { get; set; }

        public bool IsInOpenVoting { get; set; }
    }
}