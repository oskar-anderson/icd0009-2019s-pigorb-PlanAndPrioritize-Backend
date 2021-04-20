using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace BLL.App.DTO
{
    public class UserInVotingBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid AppUserId { get; set; }
        public AppUserBllDto? AppUser { get; set; }

        public Guid VotingId { get; set; }
        public VotingBllDto? Voting { get; set; }

        protected bool Equals(UserInVotingBllDto other)
        {
            return Id.Equals(other.Id) && AppUserId.Equals(other.AppUserId) && Equals(AppUser, other.AppUser) &&
                   VotingId.Equals(other.VotingId) && Equals(Voting, other.Voting);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserInVotingBllDto) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, AppUserId, AppUser, VotingId, Voting);
        }
    }
}