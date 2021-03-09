using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace DAL.App.DTO
{
    public class UserInVotingDalDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public Guid AppUserId { get; set; }
        // public AppUserDalDto? AppUser { get; set; }

        public Guid VotingId { get; set; }
        // public VotingDalDto? Voting { get; set; }
    }
}