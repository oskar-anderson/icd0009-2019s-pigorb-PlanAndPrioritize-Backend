using System;
using System.Collections.Generic;
using Classifiers;
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
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Duration { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryBllDto? Category { get; set; }
        public FeatureStatus FeatureStatus { get; set; }
        public Guid? AppUserId { get; set; }
        public AppUserBllDto? AppUser { get; set; }
        public DateTime TimeCreated { get; set; }
        
        public Guid CreatedById { get; set; }
        public AppUserBllDto? CreatedBy { get; set; }
        public DateTime LastEdited { get; set; }
        public string? ChangeLog { get; set; }
        public ICollection<CommentBllDto>? Comments { get; set; }
        public ICollection<FeatureInVotingBllDto>? FeatureInVotings { get; set; }
        public bool IsInOpenVoting { get; set; }
    }
    
    public class FeatureWithUsersPriorityBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public Guid VotingId { get; set; } = default!;

        public string Title { get; set; } = default!;

        public string? Description { get; set; }
        
        public string CategoryName { get; set; } = default!;

        public int TaskSize { get; set; }
        
        public int BusinessValue { get; set; }
        
        public int TimeCriticality { get; set; }
        
        public int RiskOrOpportunity { get; set; }
    }
}