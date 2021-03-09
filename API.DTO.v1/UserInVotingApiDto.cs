using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace API.DTO.v1
{
    public class UserInVotingApiDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public Guid AppUserId { get; set; }
        // public AppUserApiDto? AppUser { get; set; }

        public Guid VotingId { get; set; }
        // public VotingApiDto? Voting { get; set; }
    }
}