using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace BLL.App.DTO
{
    public class UserInVotingBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public Guid AppUserId { get; set; }
        // public AppUserBllDto? AppUser { get; set; }

        public Guid VotingId { get; set; }
        // public VotingBllDto? Voting { get; set; }
    }
}