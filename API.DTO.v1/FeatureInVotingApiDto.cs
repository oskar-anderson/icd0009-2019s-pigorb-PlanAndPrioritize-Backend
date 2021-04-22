using System;

namespace API.DTO.v1
{
    public class FeatureInVotingApiDto
    {
        public Guid Id { get; set; }
        
        public decimal AverageBusinessValue { get; set; }
        
        public decimal AverageTimeCriticality { get; set; }
        
        public decimal AverageRiskOrOpportunity { get; set; }
        
        public decimal AverageSize { get; set; }
        
        public decimal AveragePriorityValue { get; set; }

        public Guid VotingId { get; set; }

        public Guid FeatureId { get; set; }
    }
    
    public class FeatureInVotingCreateApiDto
    {
        public Guid FeatureId { get; set; }

        public Guid VotingId { get; set; }
    }
}