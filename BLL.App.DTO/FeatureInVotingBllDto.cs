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

        protected bool Equals(FeatureInVotingBllDto other)
        {
            return Id.Equals(other.Id) && AverageBusinessValue == other.AverageBusinessValue &&
                   AverageTimeCriticality == other.AverageTimeCriticality &&
                   AverageRiskOrOpportunity == other.AverageRiskOrOpportunity && AverageSize == other.AverageSize &&
                   AveragePriorityValue == other.AveragePriorityValue && VotingId.Equals(other.VotingId) &&
                   Equals(Voting, other.Voting) && FeatureId.Equals(other.FeatureId) &&
                   Equals(Feature, other.Feature) && Equals(UsersFeaturePriorities, other.UsersFeaturePriorities);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FeatureInVotingBllDto) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Id);
            hashCode.Add(AverageBusinessValue);
            hashCode.Add(AverageTimeCriticality);
            hashCode.Add(AverageRiskOrOpportunity);
            hashCode.Add(AverageSize);
            hashCode.Add(AveragePriorityValue);
            hashCode.Add(VotingId);
            hashCode.Add(Voting);
            hashCode.Add(FeatureId);
            hashCode.Add(Feature);
            hashCode.Add(UsersFeaturePriorities);
            return hashCode.ToHashCode();
        }
    }
}