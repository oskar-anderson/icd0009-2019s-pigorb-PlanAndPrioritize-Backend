using System;

namespace Domain
{
    public class UserInVoting
    {
        public Guid Id { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public Guid VotingId { get; set; }
        public Voting? Voting { get; set; }
    }
}