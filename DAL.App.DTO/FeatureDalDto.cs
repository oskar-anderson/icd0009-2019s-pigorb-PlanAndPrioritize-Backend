using System;
using System.Collections.Generic;
using Classifiers;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace DAL.App.DTO
{
    public class FeatureDalDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = default!;
        
        public int Size { get; set; }

        public decimal PriorityValue { get; set; }

        public string? Description { get; set; }
        
        public DateTime? StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }

        public int Duration { get; set; }

        public Guid CategoryId { get; set; }
        public CategoryDalDto? Category { get; set; }
        
        public FeatureStatus FeatureStatus { get; set; }

        public Guid? AppUserId { get; set; }
        public AppUserDalDto? AppUser { get; set; }

        public DateTime TimeCreated { get; set; }
        
        public Guid CreatedById { get; set; }
        public AppUserDalDto? CreatedBy { get; set; }
        public DateTime LastEdited { get; set; }

        public string? ChangeLog { get; set; }

        public ICollection<CommentDalDto>? Comments { get; set; }
        
        public ICollection<FeatureInVotingDalDto>? FeatureInVotings { get; set; }
    }
}