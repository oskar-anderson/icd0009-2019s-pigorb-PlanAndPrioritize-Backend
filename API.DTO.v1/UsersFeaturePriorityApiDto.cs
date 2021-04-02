using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace API.DTO.v1
{
    public class UsersFeaturePriorityApiDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public int BusinessValue { get; set; }
        
        public int TimeCriticality { get; set; }
        
        public int RiskOrOpportunity { get; set; }
        
        public int Size { get; set; }
        
        public decimal PriorityValue { get; set; }

        public string PriorityStatus { get; set; } = default!;

        public Guid AppUserId { get; set; }
        // public AppUserApiDto? AppUser { get; set; }

        public Guid FeatureInVotingId { get; set; }
        // public FeatureInVotingApiDto? FeatureInVoting { get; set; }

    }
}