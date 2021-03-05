using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace Domain
{
    public class UserInVoting : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public Guid VotingId { get; set; }
        public Voting? Voting { get; set; }
    }
}