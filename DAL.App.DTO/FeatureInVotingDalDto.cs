using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace DAL.App.DTO
{
    public class FeatureInVotingDalDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public decimal AverageBusinessValue { get; set; }
        
        public decimal AverageTimeCriticality { get; set; }
        
        public decimal AverageRiskOrOpportunity { get; set; }
        
        public decimal AverageSize { get; set; }
        
        public decimal AveragePriorityValue { get; set; }

        public Guid VotingId { get; set; }
        public VotingDalDto? Voting { get; set; }

        public Guid FeatureId { get; set; }
        // public FeatureDalDto? Feature { get; set; }

        // public ICollection<UsersFeaturePriorityDalDto>? UsersFeaturePriorities { get; set; }
    }
}