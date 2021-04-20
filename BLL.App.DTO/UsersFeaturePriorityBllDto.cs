using System;
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
        
        public Guid AppUserId { get; set; }

        public AppUserBllDto? AppUser { get; set; }

        public Guid FeatureInVotingId { get; set; }

        public FeatureInVotingBllDto? FeatureInVoting { get; set; }

        protected bool Equals(UsersFeaturePriorityBllDto other)
        {
            return Id.Equals(other.Id) && Size == other.Size && BusinessValue == other.BusinessValue &&
                   TimeCriticality == other.TimeCriticality && RiskOrOpportunity == other.RiskOrOpportunity &&
                   PriorityValue == other.PriorityValue &&
                   AppUserId.Equals(other.AppUserId) && Equals(AppUser, other.AppUser) &&
                   FeatureInVotingId.Equals(other.FeatureInVotingId) && Equals(FeatureInVoting, other.FeatureInVoting);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UsersFeaturePriorityBllDto) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Id);
            hashCode.Add(Size);
            hashCode.Add(BusinessValue);
            hashCode.Add(TimeCriticality);
            hashCode.Add(RiskOrOpportunity);
            hashCode.Add(PriorityValue);
            hashCode.Add(AppUserId);
            hashCode.Add(AppUser);
            hashCode.Add(FeatureInVotingId);
            hashCode.Add(FeatureInVoting);
            return hashCode.ToHashCode();
        }
    }
}