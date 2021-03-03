using System;

namespace Domain
{
    public class UsersFeaturePriority
    {
        public Guid Id { get; set; }

        public int BusinessValue { get; set; }
        
        public int TimeCriticality { get; set; }
        
        public int RiskOrOpportunity { get; set; }
        
        public int Size { get; set; }
        
        public decimal PriorityValue { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public Guid FeatureInVotingId { get; set; }
        public FeatureInVoting? FeatureInVoting { get; set; }

        public Guid PriorityStatusId { get; set; }
        public PriorityStatus? PriorityStatus { get; set; }
    }
}
