using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace API.DTO.v1
{
    public class FeatureInVotingApiDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public decimal AverageBusinessValue { get; set; }
        
        public decimal AverageTimeCriticality { get; set; }
        
        public decimal AverageRiskOrOpportunity { get; set; }
        
        public decimal AverageSize { get; set; }
        
        public decimal AveragePriorityValue { get; set; }

        public Guid VotingId { get; set; }
        // public VotingApiDto? Voting { get; set; }

        public Guid FeatureId { get; set; }
        // public FeatureApiDto? Feature { get; set; }

        // public ICollection<UsersFeaturePriorityApiDto>? UsersFeaturePriorities { get; set; }
    }
}