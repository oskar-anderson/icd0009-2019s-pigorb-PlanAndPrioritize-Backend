using System;
using System.Collections.Generic;
using Classifiers;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace BLL.App.DTO
{
    public class VotingBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public VotingStatus VotingStatus { get; set; }

        public ICollection<UserInVotingBllDto>? UserInVotings { get; set; }

        public ICollection<FeatureInVotingBllDto>? FeatureInVotings { get; set; }

        protected bool Equals(VotingBllDto other)
        {
            return Id.Equals(other.Id) && Title == other.Title && Description == other.Description &&
                   StartTime.Equals(other.StartTime) && EndTime.Equals(other.EndTime) &&
                   VotingStatus == other.VotingStatus && Equals(UserInVotings, other.UserInVotings) &&
                   Equals(FeatureInVotings, other.FeatureInVotings);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VotingBllDto) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, Description, StartTime, EndTime, (int) VotingStatus, UserInVotings,
                FeatureInVotings);
        }
    }

    public class VotingEditBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public VotingStatus VotingStatus { get; set; }

        public ICollection<Guid>? Users { get; set; }

        public ICollection<Guid>? Features { get; set; }
    }
}