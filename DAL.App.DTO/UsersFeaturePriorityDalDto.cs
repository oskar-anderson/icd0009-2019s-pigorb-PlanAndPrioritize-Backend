using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace DAL.App.DTO
{
    public class UsersFeaturePriorityDalDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public int BusinessValue { get; set; }
        
        public int TimeCriticality { get; set; }
        
        public int RiskOrOpportunity { get; set; }
        
        public int Size { get; set; }
        
        public decimal PriorityValue { get; set; }

        public Guid AppUserId { get; set; }
        // public AppUserDalDto? AppUser { get; set; }

        public Guid FeatureInVotingId { get; set; }
        // public FeatureInVotingDalDto? FeatureInVoting { get; set; }

        public Guid PriorityStatusId { get; set; }
        // public PriorityStatusDalDto? PriorityStatus { get; set; }
    }
}