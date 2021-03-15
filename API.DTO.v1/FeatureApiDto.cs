using System;
using System.Collections.Generic;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;
using Microsoft.VisualBasic;

namespace API.DTO.v1
{
    public class FeatureCreateApiDto
    {
        public string Title { get; set; } = default!;

        public string? Description { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }

        public int Duration { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid FeatureStatusId { get; set; }
        
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
        public string FeatureStatusName { get; set; } = default!;
        public string? Assignee { get; set; }
        
        public ICollection<Guid>? CommentIds { get; set; }
        public ICollection<Guid>? VotingIds { get; set; }
    }
}