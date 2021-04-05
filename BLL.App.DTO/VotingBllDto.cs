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
    }
}