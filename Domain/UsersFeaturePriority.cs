using System;
using System.ComponentModel.DataAnnotations.Schema;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace Domain
{
    public class UsersFeaturePriority : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public int BusinessValue { get; set; }
        
        public int TimeCriticality { get; set; }
        
        public int RiskOrOpportunity { get; set; }
        
        public int Size { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal PriorityValue { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public Guid FeatureInVotingId { get; set; }
        public FeatureInVoting? FeatureInVoting { get; set; }
    }
}
