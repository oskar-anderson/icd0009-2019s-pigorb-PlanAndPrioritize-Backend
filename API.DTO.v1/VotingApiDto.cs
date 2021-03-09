using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace API.DTO.v1
{
    public class VotingApiDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }

        public Guid VotingStatusId { get; set; }
        // public VotingStatusApiDto? VotingStatus { get; set; }
        //
        // public ICollection<UserInVotingApiDto>? UserInVotings { get; set; }
        //
        // public ICollection<FeatureInVotingApiDto>? FeatureInVotings { get; set; }
    }
}