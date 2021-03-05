using System;
using System.Collections.Generic;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace Domain
{
    public class Voting : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }

        public Guid VotingStatusId { get; set; }
        public VotingStatus? VotingStatus { get; set; }

        public ICollection<UserInVoting>? UserInVotings { get; set; }
        
        public ICollection<FeatureInVoting>? FeatureInVotings { get; set; }
    }
}
