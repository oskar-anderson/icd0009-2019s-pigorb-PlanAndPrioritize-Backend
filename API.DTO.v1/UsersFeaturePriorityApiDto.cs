using System;

namespace API.DTO.v1
{
    public class UsersFeaturePriorityCreateApiDto
    {
        public int TaskSize { get; set; }
        
        public int BusinessValue { get; set; }
        
        public int TimeCriticality { get; set; }
        
        public int RiskOrOpportunity { get; set; }

        public Guid Id { get; set; } // FeatureId, sent initially via FeatureApiDto, filled and sent back
        
        public Guid VotingId { get; set; }
    }
    
    public class UsersFeaturePriorityApiDto
    {
        public Guid Id { get; set; }
        
        public int Size { get; set; }
        
        public int BusinessValue { get; set; }
        
        public int TimeCriticality { get; set; }
        
        public int RiskOrOpportunity { get; set; }

        public decimal PriorityValue { get; set; }
        
        public string UserName { get; set; } = default!;
    }
}