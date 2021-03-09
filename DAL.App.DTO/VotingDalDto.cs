using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace DAL.App.DTO
{
    public class VotingDalDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }

        public Guid VotingStatusId { get; set; }
        // public VotingStatusDalDto? VotingStatus { get; set; }
        //
        // public ICollection<UserInVotingDalDto>? UserInVotings { get; set; }
        //
        // public ICollection<FeatureInVotingDalDto>? FeatureInVotings { get; set; }
    }
}