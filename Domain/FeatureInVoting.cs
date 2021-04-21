using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace Domain
{
    public class FeatureInVoting : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal AverageBusinessValue { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal AverageTimeCriticality { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal AverageRiskOrOpportunity { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal AverageSize { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal AveragePriorityValue { get; set; }

        public Guid VotingId { get; set; }
        public Voting? Voting { get; set; }

        public Guid FeatureId { get; set; }
        public Feature? Feature { get; set; }

        public ICollection<UsersFeaturePriority>? UsersFeaturePriorities { get; set; }
    }
}