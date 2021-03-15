using System;
using System.Collections.Generic;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace BLL.App.DTO
{
    public class FeatureBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = default!;
        
        public int Size { get; set; }

        public decimal PriorityValue { get; set; }

        public string? Description { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }

        public int Duration { get; set; }

        public Guid? CategoryId { get; set; }
        public CategoryBllDto? Category { get; set; }
        
        public Guid FeatureStatusId { get; set; }
        public FeatureStatusBllDto? FeatureStatus { get; set; }
        
        public Guid? AppUserId { get; set; }
        public AppUserBllDto? AppUser { get; set; }

        public DateTime TimeCreated { get; set; }
        
        public DateTime LastEdited { get; set; }

        public string? ChangeLog { get; set; }

        public ICollection<CommentBllDto>? Comments { get; set; }
        
        public ICollection<FeatureInVotingBllDto>? FeatureInVotings { get; set; }
    }
}