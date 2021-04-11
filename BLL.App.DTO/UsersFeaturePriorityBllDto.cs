using System;
using Classifiers;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace BLL.App.DTO
{
    public class UsersFeaturePriorityBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public int Size { get; set; }

        public int BusinessValue { get; set; }
        
        public int TimeCriticality { get; set; }
        
        public int RiskOrOpportunity { get; set; }

        public decimal PriorityValue { get; set; }
        
        public PriorityStatus PriorityStatus { get; set; } // Should be removed or is necessary?

        public Guid AppUserId { get; set; }

        public Guid FeatureInVotingId { get; set; }
    }
}