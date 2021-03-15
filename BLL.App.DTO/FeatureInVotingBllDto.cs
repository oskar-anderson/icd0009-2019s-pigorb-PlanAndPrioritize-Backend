using System;
using System.Collections.Generic;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace BLL.App.DTO
{
    public class FeatureInVotingBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public decimal AverageBusinessValue { get; set; }
        
        public decimal AverageTimeCriticality { get; set; }
        
        public decimal AverageRiskOrOpportunity { get; set; }
        
        public decimal AverageSize { get; set; }
        
        public decimal AveragePriorityValue { get; set; }

        public Guid VotingId { get; set; } 
        public VotingBllDto? Voting { get; set; }

        public Guid FeatureId { get; set; }
        public FeatureBllDto? Feature { get; set; }

        public ICollection<UsersFeaturePriorityBllDto>? UsersFeaturePriorities { get; set; }
    }
}