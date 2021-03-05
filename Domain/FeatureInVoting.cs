using System;
using System.Collections.Generic;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace Domain
{
    public class FeatureInVoting : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public decimal AverageBusinessValue { get; set; }
        
        public decimal AverageTimeCriticality { get; set; }
        
        public decimal AverageRiskOrOpportunity { get; set; }
        
        public decimal AverageSize { get; set; }
        
        public decimal AveragePriorityValue { get; set; }

        public Guid VotingId { get; set; }
        public Voting? Voting { get; set; }

        public Guid FeatureId { get; set; }
        public Feature? Feature { get; set; }

        public ICollection<UsersFeaturePriority>? UsersFeaturePriorities { get; set; }
    }
}